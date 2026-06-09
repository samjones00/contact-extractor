using ContactExtractor.Core.Interfaces;
using ContactExtractor.Core.Models;
using ContactExtractor.Core.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ContactExtractor.Core
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            var settings = configuration.GetSection(Settings.SectionName).Get<Settings>() ?? new Settings();

            services.AddSingleton(settings);
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient<IHtmlContactParser, HtmlContactParser>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddHttpClient<SearchService>(nameof(SearchService), x =>
            {
                x.BaseAddress = new Uri(settings.Url);
                x.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            });

            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
        }
    }
}
