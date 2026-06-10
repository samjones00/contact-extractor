using ContactExtractor.Core.Services;
using FluentAssertions;

namespace ContactExtractor.UnitTests.Services
{
    public class HtmlContactParserTests
    {
        private HtmlContactParser _service;

        [SetUp]
        public void Setup()
        {
            _service = new HtmlContactParser();
        }

        [Test]
        public void GivenHtml_LargeContent_ShouldReturnContacts()
        {
            var htmlContent = GetHtmlContent();
            var response = _service.Parse(htmlContent);

            response = response.Take(63).ToList();

            response.Count.Should().BeGreaterThan(0);

            for (int i = 0; i < response.Count; i++)
            {
                var because = $"Number: {i}";
                response[i].Name.Should().NotBeNullOrEmpty(because);
                response[i].Telephone.Should().NotBeNullOrEmpty(because);
                response[i].Address.Should().NotBeNullOrEmpty(because);
            }
        }

        [Test]
        public void GivenHtml__SmallContent_ShouldReturnContacts()
        {
            var htmlContent = GetHtmlContent();
            var response = _service.Parse(htmlContent);

            response = [.. response.Skip(63)];

            response.Count.Should().BeGreaterThan(0);

            for (int i = 0; i < response.Count; i++)
            {
                var because = $"Number: {i}";
                response[i].Name.Should().NotBeNullOrEmpty(because);
                response[i].Telephone.Should().NotBeNullOrEmpty(because);
                response[i].Address.Should().NotBeNullOrEmpty(because);
            }
        }

        [Test]
        public void GivenEmptyHtml_ShouldReturnEmptyList()
        {
            var result = _service.Parse(string.Empty);

            result.Should().BeEmpty();
        }

        [Test]
        public void GivenHtmlWithNoResultItems_ShouldReturnEmptyList()
        {
            var html = "<html><body><div class='other'>no results</div></body></html>";

            var result = _service.Parse(html);

            result.Should().BeEmpty();
        }

        [Test]
        public void GivenHtml_SingleRegularItem_ShouldParseAllFields()
        {
            var html = """
                <div class="result-item">
                    <div class="top-holder">
                        <span class="h2">
                            Test Solicitors
                            <div class="greentick" title="Quality mark"></div>
                        </span>
                        <div class="phone-block mobile-hidden">
                            <span>Phone:</span>
                            <a rel="noindex" href="tel:02071234567">0207 123 4567</a>
                        </div>
                    </div>
                    <a href="/test-solicitors.html" class="link-map"><i class="fa fa-map-marker"></i><address>123 Test Street, London SW1A 1AA</address></a>
                    <p>We provide expert legal advice.</p>
                    <ul class="list-item">
                        <li><a target="_blank" href="https://www.testsolicitors.co.uk" rel="nofollow">Website</a></li>
                    </ul>
                </div>
                """;

            var result = _service.Parse(html);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Test Solicitors");
            result[0].Telephone.Should().Be("0207 123 4567");
            result[0].Address.Should().Be("123 Test Street, London SW1A 1AA");
            result[0].Description.Should().Be("We provide expert legal advice");
            result[0].Url.Should().Be("https://www.testsolicitors.co.uk");
        }

        [Test]
        public void GivenHtml_SingleSmallItem_ShouldParseAllFields()
        {
            var html = """
                <div class="result-item item-small">
                    <span class="h2">Small Solicitors</span>
                    <a href="/small-solicitors.html" class="link-map"><i class="fa fa-map-marker"></i><address>456 Small Road, London SE1 1AA</address></a>
                    <a class="tel" rel="noindex" href="tel:02079876543">0207 987 6543</a>
                    <p>Small firm expertise.</p>
                </div>
                """;

            var result = _service.Parse(html);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Small Solicitors");
            result[0].Telephone.Should().Be("0207 987 6543");
            result[0].Address.Should().Be("456 Small Road, London SE1 1AA");
            result[0].Description.Should().Be("Small firm expertise");
        }

        [Test]
        public void GivenHtml_ItemMissingName_ShouldReturnEmptyName()
        {
            var html = """
                <div class="result-item">
                    <div class="top-holder">
                        <div class="phone-block mobile-hidden">
                            <a rel="noindex" href="tel:02071234567">0207 123 4567</a>
                        </div>
                    </div>
                    <a href="/test.html" class="link-map"><address>123 Test Street</address></a>
                    <p>Description text.</p>
                </div>
                """;

            var result = _service.Parse(html);

            result.Should().HaveCount(1);
            result[0].Name.Should().BeEmpty();
            result[0].Telephone.Should().Be("0207 123 4567");
        }

        [Test]
        public void GivenHtml_ItemMissingPhone_ShouldReturnEmptyPhone()
        {
            var html = """
                <div class="result-item">
                    <div class="top-holder">
                        <span class="h2">No Phone Co</span>
                    </div>
                    <a href="/test.html" class="link-map"><address>123 Test Street</address></a>
                    <p>Description text.</p>
                </div>
                """;

            var result = _service.Parse(html);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("No Phone Co");
            result[0].Telephone.Should().BeEmpty();
        }

        [Test]
        public void GivenHtml_ItemMissingAddress_ShouldReturnEmptyAddress()
        {
            var html = """
                <div class="result-item">
                    <div class="top-holder">
                        <span class="h2">No Address Co</span>
                        <div class="phone-block mobile-hidden">
                            <a rel="noindex" href="tel:02071234567">0207 123 4567</a>
                        </div>
                    </div>
                    <p>Description text.</p>
                </div>
                """;

            var result = _service.Parse(html);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("No Address Co");
            result[0].Address.Should().BeEmpty();
        }

        [Test]
        public void GivenHtml_ItemWithExtraWhitespaceInName_ShouldNormalize()
        {
            var html = """
                <div class="result-item">
                    <div class="top-holder">
                        <span class="h2">
                            Smith


                            Jones Solicitors
                        </span>
                    </div>
                    <a href="/test.html" class="link-map"><address>123 Test Street</address></a>
                </div>
                """;

            var result = _service.Parse(html);

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Smith Jones Solicitors");
            result[0].Address.Should().Be("123 Test Street");
        }

        private static string GetHtmlContent() => File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"services\artifacts\search-response.html"));
    }
}