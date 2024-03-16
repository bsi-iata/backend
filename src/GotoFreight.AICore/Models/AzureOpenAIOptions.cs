namespace GotoFreight.AICore.Models
{
    public class AzureOpenAIOptions
    {
        /// <summary>
        /// ex: gpt-4-version-preview
        /// </summary>
        public string VersionPreviewDeploymentName { get; set; } = null!;

        /// <summary>
        /// max tokens
        /// </summary>
        public int VersionPreviewMaxTokens { get; set; }

        /// <summary>
        /// ex: gpt-4-32k
        /// </summary>
        public string ChatCompletionDeploymentName { get; set; } = null!;

        /// <summary>
        /// max tokens
        /// </summary>
        public int ChatCompletionMaxTokens { get; set; }

        /// <summary>
        /// ex: https://{resource name}.openai.azure.com
        /// </summary>
        public string Endpoint { get; set; } = null!;

        /// <summary>
        /// ApiKey
        /// </summary>
        public string ApiKey { get; set; } = null!;
    }
}
