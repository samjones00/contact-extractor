namespace ContactExtractor.UnitTests.ExtractorService
{
    using System.Runtime.CompilerServices;
    using Api.Services;

    public class Tests
    {
        private ExtractorService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ExtractorService();
        }

        [Test]
        public void Test1()
        {
            //string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "search-response.html");
            var filePath = GetArtifactFilename(@"artifacts\search-response.html");
            // 2. Read the file
            string htmlContent = File.ReadAllText(filePath);

            var response = _service.ExtractContacts(htmlContent);
        }

        private static string GetArtifactFilename(string filename,[CallerFilePath] string sourceFilePath = "" )
        {
            return Path.Combine(Path.GetDirectoryName(sourceFilePath), filename);
        }
    }
}