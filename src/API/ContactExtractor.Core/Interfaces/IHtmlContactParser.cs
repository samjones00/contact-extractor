using ContactExtractor.Core.Models;

namespace ContactExtractor.Core.Interfaces
{
    public interface IHtmlContactParser
    {
        List<Contact> ExtractContacts(string html);
    }
}