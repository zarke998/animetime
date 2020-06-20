using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace AnimeTimeDbUpdater
{
    public class TestApplication : IApplication
    {
        public void Run()
        {
            BlobClient b = new BlobClient("DefaultEndpointsProtocol=https;AccountName=animetime;AccountKey=vS1Z/ZAhE7S08n92xLwYGgH7xKkcA8F8DrLQnnkH3YefEF5GQeLJ/+XsCaI0O9rSeWhTcsY0bwlDDYYiFKfzTA==;EndpointSuffix=core.windows.net","images","fma.jpg");

            b.Upload("C:\\Users\\zarke\\Desktop\\Unity Tutorial link.txt");

            Console.WriteLine("Blob uploading done.");
        }
    }
}
