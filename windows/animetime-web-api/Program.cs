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
            var serverAddress = "http://localhost:9000/";

            using (var app = WebApp.Start<Startup>(serverAddress))
            {
                var client = new HttpClient();

                var response = client.GetAsync(serverAddress + "api/").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }
        }
    }
}