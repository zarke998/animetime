using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AnimeTime.WebAPI.MessageHandlers
{
    public class ApiKeyHandler : DelegatingHandler
    {
        private const string _key = @"ZsamuKFuj7crELCvmPcJv34o0EVfJctrbLxG11y8b7Yd8GESKL8LbRsDtZyk3Gr70PJP9v0zS1LRt7QIfUGwCNf5Kq863fIhkp74";


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (!ValidateKey(request))
            {
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("No API key provided. Please provide a key via 'api-key' custom header.")
                };
                var task = new TaskCompletionSource<HttpResponseMessage>();
                task.SetResult(errorResponse);
                return task.Task;
            }

            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidateKey(HttpRequestMessage request)
        {
            
            if(request.Headers.TryGetValues("api-key", out var headerValues))
            {
                return headerValues.Contains(_key);
            }

            return false;
        }
    }
}