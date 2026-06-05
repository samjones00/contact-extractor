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
            services.AddHttpClient<SearchService>(x =>
            {
                x.BaseAddress = new Uri("https://www.solicitors.com/prepare-search.asp");
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

            return services;
        }
    }
}
