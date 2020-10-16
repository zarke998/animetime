using AnimeTimeDbUpdater.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.IO;
using AnimeTime.Utilities.Imaging;
using Azure;
using Azure.Storage.Blobs.Models;
using AnimeTimeDbUpdater.Utilities;
using System.Diagnostics;

namespace AnimeTimeDbUpdater.Persistence
{
    public class AzureImageStorage : IImageStorage
    {
        static HashSet<long> _generatedIds;

        private const string _connectionString = "DefaultEndpointsProtocol=https;AccountName=animetime;AccountKey=vS1Z/ZAhE7S08n92xLwYGgH7xKkcA8F8DrLQnnkH3YefEF5GQeLJ/+XsCaI0O9rSeWhTcsY0bwlDDYYiFKfzTA==;EndpointSuffix=core.windows.net";

        private const string _thumbnailContainerName = "thumb";
        private static BlobContainerClient _thumbnailContainer;

        static AzureImageStorage()
        {
            _thumbnailContainer = new BlobContainerClient(_connectionString, _thumbnailContainerName);
            _generatedIds = new HashSet<long>();
        }
        private static HashSet<int> GetGeneratedIds()
        {
            ICollection<int> ids = new List<int>();

            var blobItems = _thumbnailContainer.GetBlobs().AsEnumerable();
            var blobs = blobItems.Select(bi => bi.Name);

            foreach (var b in blobs)
            {
                var id = Convert.ToInt32(b.Split('.')[0]);
                ids.Add(id);
            }

            return new HashSet<int>(ids);
        }

        public IEnumerable<string> Upload(IEnumerable<Image<Rgba32>> images)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(Image<Rgba32> image)
        {
            var stream = await image.ToStreamAsync();

            string url = null;

            for (int i = 0; i < 10; i++)
            {
                long timestamp;
                do
                {
                    timestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                } while (_generatedIds.Contains(timestamp));

                try
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    var blob = _thumbnailContainer.GetBlobClient($"{timestamp}.jpg");
                    await blob.UploadAsync(stream);

                    _generatedIds.Add(timestamp);
                    url = blob.Uri.AbsoluteUri;

                    break;
                }
                catch(RequestFailedException uploadException) // Upload failed
                {
                    LogGroup.Log($"Error uploading blob. Retry attempt {i + 1}.");
                    Debug.WriteLine("Generated timestamp: {0}", timestamp);
                    Debug.WriteLine(uploadException.Message);
                }
            }

            return url;
        }
        public async Task<IEnumerable<string>> UploadAsync(IEnumerable<Image<Rgba32>> images)
        {
            // Convert images to streams
            ICollection<Task<Stream>> imageToStreamTasks = new List<Task<Stream>>();
            foreach (var image in images)
            {
                imageToStreamTasks.Add(image.ToStreamAsync());
            }
            var imageStreams = await Task.WhenAll(imageToStreamTasks);



            ICollection<string> urls = new List<string>();
            ICollection<long> newIds = new List<long>();

            ICollection<Stream> uploadedStreams = new List<Stream>();

            try
            {
                await UploadStreams(imageStreams);
            }
            catch (RequestFailedException requestFailedException) // Blob exists with generated name
            {
                var notUploadedStreams = imageStreams.Except(uploadedStreams);
                try
                {
                    await UploadStreams(notUploadedStreams);

                }
                catch(RequestFailedException requestFailedSecondException)
                {
                    return null;
                }
            }
            _generatedIds.UnionWith(newIds);

            return urls;

            async Task UploadStreams(IEnumerable<Stream> streams)
            {
                ICollection<Task> uploadTasks = new List<Task>();

                foreach (var stream in streams)
                {
                    long timestamp;
                    do
                    {
                        timestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                    } while (_generatedIds.Contains(timestamp) || newIds.Contains(timestamp));

                    var blob = _thumbnailContainer.GetBlobClient($"{timestamp}.jpg");
                    uploadTasks.Add(blob.UploadAsync(stream).
                        ContinueWith((t) =>
                        {
                            urls.Add(blob.Uri.AbsoluteUri);
                            newIds.Add(timestamp);
                            uploadedStreams.Add(stream);
                        }));
                }

                await Task.WhenAll(uploadTasks);
            }
        }
    }
}
