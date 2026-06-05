using System.Runtime.CompilerServices;
using ContactExtractor.Core.Services;

namespace ContactExtractor.UnitTests.ExtractorServiceTests
{
    public class Tests
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

            Assert.AreNotEqual(0, response.Count);
        }
    }
}