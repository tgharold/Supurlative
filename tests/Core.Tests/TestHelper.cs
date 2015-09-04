﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RimDev.Supurlative.Tests
{
    public static class TestHelper
    {

        public static TemplateGenerator CreateATemplateGenerator(
            string baseUrl,
            string routeName,
            string routeTemplate,
            object routeDefaults = null,
            object routeConstraints = null,
            SupurlativeOptions supurlativeOptions = null
            )
        {
            HttpRouteCollection routes = new HttpRouteCollection();
            routes.MapHttpRoute(
                routeName,
                routeTemplate,
                defaults: routeDefaults,
                constraints: routeConstraints
                );
            HttpConfiguration configuration = new HttpConfiguration(routes);
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseUrl),
                Method = HttpMethod.Get
            };
            request.SetConfiguration(configuration);
            return new TemplateGenerator(request, supurlativeOptions);
        }

        public static UrlGenerator CreateAUrlGenerator(
            string baseUrl,
            string routeName,
            string routeTemplate,
            object routeDefaults = null,
            object routeConstraints = null,
            SupurlativeOptions supurlativeOptions = null
            )
        {
            HttpRouteCollection routes = new HttpRouteCollection();
            routes.MapHttpRoute(
                routeName,
                routeTemplate,
                defaults: routeDefaults,
                constraints: routeConstraints
                );
            HttpConfiguration configuration = new HttpConfiguration(routes);
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseUrl),
                Method = HttpMethod.Get
            };
            request.SetConfiguration(configuration);
            return new UrlGenerator(request, supurlativeOptions);
        }

    }
}
