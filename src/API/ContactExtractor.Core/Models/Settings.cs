namespace ContactExtractor.Core.Models
{
    public record Settings
    {
        public const string SectionName = nameof(Settings);

        public string Url { get; init; } = string.Empty;
        public List<string> AllowedLocations { get; init; } = [];
    }
}