using AnimeTime.Core.Domain;
using AnimeTime.Persistence;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Console.ReadLine();
            }
        }
    }
}