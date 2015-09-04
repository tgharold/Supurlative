using System;
using System.Net.Http;
using System.Web.Http;
using Xunit;

namespace RimDev.Supurlative.Tests
{
    public class UrlGeneratorTests
    {
        public UrlGeneratorTests()
        {
            Generator = InitializeGenerator();
        }

        private readonly UrlGenerator Generator;

        private static UrlGenerator InitializeGenerator(SupurlativeOptions options = null)
        {
            var request = WebApiHelper.GetRequest();
            return new UrlGenerator(request);
        }

        const string _baseURL = "http://localhost:8000/";

        private static UrlGenerator CreateAUrlGenerator(
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
                RequestUri = new Uri(_baseURL),
                Method = HttpMethod.Get
            };
            request.SetConfiguration(configuration);
            return new UrlGenerator(request, supurlativeOptions);
        }

        [Fact]
        public void Can_generate_a_fully_qualified_path()
        {
            string expected = _baseURL + "foo/1";
            const string routeName = "foo.show";
            const string routeTemplate = "foo/{id}";
            string actual = CreateAUrlGenerator(routeName, routeTemplate)
                .Generate(routeName, new { Id = 1 });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Can_generate_a_path_with_anonymous_complex_route_properties()
        {
            var expected = "http://localhost:8000/foo/1?bar.abc=abc&bar.def=def";

            var actual = Generator.Generate("foo.show", new { Id = 1, Bar = new { Abc = "abc", Def = "def" } });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Cannot_generate_a_path_with_invalid_constraints()
        {
            string expected = null;
            var actual = Generator.Generate("constraint", new { Id = "abc" });

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Can_generate_two_optional_path_items_template()
        {
            string expected = _baseURL + "foo/1";
            const string routeName = "foo.one.two";
            const string routeTemplate = "foo/{one}/{two}";
            string actual = CreateAUrlGenerator(routeName, routeTemplate,
                routeDefaults: new { one = RouteParameter.Optional, two = RouteParameter.Optional })
                .Generate(routeName, new { one = 1 });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Can_generate_two_optional_path_items_template_two()
        {
            string expected = _baseURL + "foo/1/2";
            const string routeName = "foo.one.two";
            const string routeTemplate = "foo/{one}/{two}";
            string actual = CreateAUrlGenerator(routeName, routeTemplate,
                routeDefaults: new { one = RouteParameter.Optional, two = RouteParameter.Optional })
                .Generate(routeName, new { one = 1, two = 2 });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Make_sure_null_nested_class_property_values_do_not_show_in_url()
        {
            string expected = _baseURL + "foo/1";
            const string routeName = "foo.show";
            const string routeTemplate = "foo/{id}";
            string actual = CreateAUrlGenerator(routeName, routeTemplate)
                .Generate(routeName, new TestNestedClass { Id = 1 });
            Assert.Equal(expected, actual);
        }

        public class TestNestedClass
        {
            public int Id { get; set; }

            public NestedClass Filter { get; set; }

            public class NestedClass
            {
                public int Level { get; set; }
            }
        }

    }
}
