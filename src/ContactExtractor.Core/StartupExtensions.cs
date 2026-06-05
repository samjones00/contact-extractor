using ContactExtractor.Core.Models;
using ContactExtractor.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContactExtractor.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddContactServices(this IServiceCollection services)
        {
            services.AddTransient<HtmlContactParser>();
            services.AddTransient<SearchService>();
            services.AddHttpClient<SearchService>();
            services.AddSingleton(x => new SearchSettings
            {
                Url = "https://www.solicitors.com/prepare-search.asp"
            });
            return services;
        }
    }
}
