using GotoFreight.AICore.Services;
using GotoFreight.AICore.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace GotoFreight.AICore
{
    public static class AICoreExtensions
    {
        /// <summary>
        /// Register Azure Open AI Services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzureOpenAICoreService(this IServiceCollection services)
        {
            services.AddScoped<IImageDetectionService, AzureOpenAIImageDetectionService>();
            return services;
        }

        /// <summary>
        /// Register Open AI Services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenAICoreService(this IServiceCollection services)
        {
            services.AddScoped<IImageDetectionService, OpenAIImageDetectionService>();
            return services;
        }
    }
}
