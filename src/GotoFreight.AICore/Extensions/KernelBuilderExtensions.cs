using GotoFreight.AICore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace GotoFreight.AICore.Extensions
{
    public static class KernelBuilderExtensions
    {
        /// <summary>
        /// Use ChatCompletion model
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IKernelBuilder WithAzureOpenAIChat(this IKernelBuilder builder, AzureOpenAIOptions options)
        {
            builder.Services.AddLogging(c =>
            {
                c
                .SetMinimumLevel(LogLevel.Information)
                .AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                });
            });

            builder.Services.AddAzureOpenAIChatCompletion(
                options.ChatCompletionDeploymentName,
                options.Endpoint,
                options.ApiKey
            );
            return builder;
        }

        /// <summary>
        /// Use visual models,ex gpt-4-version-preview
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IKernelBuilder WithAzureOpenAIVersion(this IKernelBuilder builder, AzureOpenAIOptions options)
        {
            builder.Services.AddLogging(c =>
            {
                c
                .SetMinimumLevel(LogLevel.Information)
                .AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                });
            });

            builder.Services.AddAzureOpenAIChatCompletion(
                options.VersionPreviewDeploymentName,
                options.Endpoint,
                options.ApiKey
            );
            return builder;
        }



        /// <summary>
        /// Use ChatCompletion model
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IKernelBuilder WithOpenAIChat(this IKernelBuilder builder, OpenAIOptions options)
        {
            builder.Services.AddLogging(c =>
            {
                c
                .SetMinimumLevel(LogLevel.Information)
                .AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                });
            });

            builder.Services.AddOpenAIChatCompletion(
                options.ChatCompletionDeploymentName,
                options.ApiKey,
                options.AIOrgId
            );
            return builder;
        }

        /// <summary>
        /// Use visual models,ex gpt-4-version-preview
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IKernelBuilder WithOpenAIVersion(this IKernelBuilder builder, OpenAIOptions options)
        {
            builder.Services.AddLogging(c =>
            {
                c
                .SetMinimumLevel(LogLevel.Information)
                .AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                });
            });

            builder.Services.AddOpenAIChatCompletion(
                options.VersionPreviewDeploymentName,
                options.ApiKey,
                options.AIOrgId
            );
            return builder;
        }
    }
}
