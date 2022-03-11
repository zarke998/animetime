using AnimeTime.WebAPI.MessageHandlers;
using Autofac.Integration.WebApi;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AnimeTime.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var configuration = new HttpConfiguration();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(ContainerConfiguration.Configure());

            configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            configuration.MessageHandlers.Add(new ApiKeyHandler());

            configuration.MapHttpAttributeRoutes();


            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(configuration);
        }
    }
}