using ContactExtractor.Core.Models;
using ContactExtractor.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ContactExtractor.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddContactServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddTransient<HtmlContactParser>();
            services.AddTransient<SearchService>();
            services.AddHttpClient<SearchService>();
            services.AddSingleton(x => new SearchSettings
            {
                Url = "https://www.solicitors.com/prepare-search.asp"
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

            return services;
        }
    }
}
