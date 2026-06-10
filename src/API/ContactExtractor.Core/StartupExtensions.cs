using ContactExtractor.Core.Interfaces;
using ContactExtractor.Core.Models;
using ContactExtractor.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactExtractor.Core
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ?? new Settings();

            services.AddSingleton(settings);
            services.AddTransient<IHtmlContactParser, HtmlContactParser>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddHttpClient<SearchService>(nameof(SearchService), x =>
            {
                x.BaseAddress = new Uri(settings.Url);
                x.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            });
        }
    }
}
