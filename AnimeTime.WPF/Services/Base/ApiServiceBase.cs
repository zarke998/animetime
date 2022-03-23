using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.WPF.Services.Base
{
    public abstract class ApiServiceBase
    {
        protected readonly HttpClient _httpClient;

        public ApiServiceBase()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:9000/"),
            };
            _httpClient.DefaultRequestHeaders.Add("api-key", "ZsamuKFuj7crELCvmPcJv34o0EVfJctrbLxG11y8b7Yd8GESKL8LbRsDtZyk3Gr70PJP9v0zS1LRt7QIfUGwCNf5Kq863fIhkp74");
        }
    }
}
