using GotoFreight.AICore.Models;
using Microsoft.Extensions.Configuration;

namespace GotoFreight.AICore.Helpers
{
    public static class AIOptionHelper
    {
        public static AzureOpenAIOptions BuildAzureOpenAIOptions(IConfiguration configuration)
        {
            var azureOpenAIEndpoint = configuration["AzureOpenAI:Endpoint"]!;
            var azureOpenAIApiKey = configuration["AzureOpenAI:ApiKey"]!;

            var versionPreviewDeploymentName = configuration["AzureOpenAI:VersionPreviewDeploymentName"]!;
            var chatCompletionDeploymentName = configuration["AzureOpenAI:ChatCompletionDeploymentName"]!;

            var versionPreviewMaxTokens = configuration.GetValue<int>("AzureOpenAI:VersionPreviewMaxTokens")!;
            var chatCompletionMaxTokens = configuration.GetValue<int>("AzureOpenAI:ChatCompletionMaxTokens")!;

            return new AzureOpenAIOptions
            {
                Endpoint = azureOpenAIEndpoint,
                ApiKey = azureOpenAIApiKey,
                VersionPreviewDeploymentName = versionPreviewDeploymentName,
                ChatCompletionDeploymentName = chatCompletionDeploymentName,
                VersionPreviewMaxTokens = versionPreviewMaxTokens,
                ChatCompletionMaxTokens = chatCompletionMaxTokens
            };
        }

        public static OpenAIOptions BuildOpenAIOptions(IConfiguration configuration)
        {
            var OpenAIApiKey = configuration["OpenAI:OpenAIApiKey"]!;
            var OpenAIOrgId = configuration["OpenAI:OpenAIOrgId"]!;

            var versionPreviewDeploymentName = configuration["OpenAI:VersionPreviewDeploymentName"]!;
            var chatCompletionDeploymentName = configuration["OpenAI:ChatCompletionDeploymentName"]!;

            var versionPreviewMaxTokens = configuration.GetValue<int>("OpenAI:VersionPreviewMaxTokens")!;
            var chatCompletionMaxTokens = configuration.GetValue<int>("OpenAI:ChatCompletionMaxTokens")!;

            return new OpenAIOptions
            {
                ApiKey = OpenAIApiKey,
                AIOrgId = OpenAIOrgId,
                VersionPreviewDeploymentName = versionPreviewDeploymentName,
                ChatCompletionDeploymentName = chatCompletionDeploymentName,
                VersionPreviewMaxTokens = versionPreviewMaxTokens,
                ChatCompletionMaxTokens = chatCompletionMaxTokens
            };
        }
    }
}
