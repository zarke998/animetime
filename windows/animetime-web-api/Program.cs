using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AnimeTime.WebAPI
{
    public class Program
    {
        public static void Main()
        {
            var apiKey = @"ZsamuKFuj7crELCvmPcJv34o0EVfJctrbLxG11y8b7Yd8GESKL8LbRsDtZyk3Gr70PJP9v0zS1LRt7QIfUGwCNf5Kq863fIhkp74";
            var serverAddress = "http://localhost:9000/";

            using (var app = WebApp.Start<Startup>(serverAddress))
            {
                var client = new HttpClient();

                var request = new HttpRequestMessage()
                { 
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(serverAddress + "api/animes/130/episodes-with-sources")
                };
                request.Headers.Add("api-key", apiKey);

                Console.WriteLine(request);

                var response = client.SendAsync(request).Result;


                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }
        }
    }
}