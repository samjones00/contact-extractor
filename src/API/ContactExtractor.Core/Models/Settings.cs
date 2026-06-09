namespace ContactExtractor.Core.Models
{
    public class Settings
    {
        public const string SectionName = nameof(Settings);

        public string Url { get; set; } = string.Empty;
        public List<string> AllowedLocations { get; set; } = [];
    }
}