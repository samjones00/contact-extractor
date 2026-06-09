using System.Runtime.CompilerServices;
using System.Security;
using ContactExtractor.Core.Services;
using FluentAssertions;

namespace ContactExtractor.UnitTests.Services
{
    public class HtmlContactParserServiceTests
    {
        private HtmlContactParser _service;

        [SetUp]
        public void Setup()
        {
            _service = new HtmlContactParser();
        }

        [Test]
        public void GivenHtml_ShouldReturnContacts()
        {
            var htmlContent = GetHtmlContent();
            var response = _service.Parse(htmlContent);

            response = response.Take(50).ToList();
          
            response.Count.Should().BeGreaterThan(0);

            for (int i = 0; i < response.Count; i++)
            {
                var because = $"Number: {i}";
                response[i].Name.Should().NotBeNullOrEmpty(because);
                response[i].Telephone.Should().NotBeNullOrEmpty(because);
                response[i].Address.Should().NotBeNullOrEmpty(because);
            }
        }

        private static string GetHtmlContent() => File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"services\artifacts\search-response.html"));
    }
}