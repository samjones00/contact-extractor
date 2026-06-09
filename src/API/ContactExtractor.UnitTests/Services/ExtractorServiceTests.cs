using System.Runtime.CompilerServices;
using ContactExtractor.Core.Services;
using FluentAssertions;

namespace ContactExtractor.UnitTests.ExtractorServiceTests
{
    public class ExtractorServiceTests
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
            string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"extractorservicetests\artifacts\search-response.html");
            string htmlContent = File.ReadAllText(filePath);

            var response = _service.ExtractContacts(htmlContent);

            for (int i = 0; i < response.Count; i++)
            {
                response[i].Name.Should().NotBeNullOrEmpty($"Number: {i}");
                //contact.Telephone.Should().NotBeNullOrEmpty();
                //contact.Address.Should().NotBeNullOrEmpty();
            }

            response.Count.Should().BeGreaterThan(0);
        }
    }
}