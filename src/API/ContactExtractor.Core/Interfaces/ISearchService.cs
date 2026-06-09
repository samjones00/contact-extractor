namespace ContactExtractor.Core.Interfaces
{
    public interface ISearchService
    {
        Task<string> Search(string location, CancellationToken cancellationToken);
    }
}