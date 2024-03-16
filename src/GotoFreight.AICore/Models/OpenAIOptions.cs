namespace GotoFreight.AICore.Models
{
    public class OpenAIOptions
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
        /// ApiKey
        /// </summary>
        public string ApiKey { get; set; } = null!;

        /// <summary>
        /// OpenAI organization id. This is usually optional unless your account belongs to multiple organizations.
        /// </summary>
        public string? AIOrgId { get; set; } = null!;
    }
}
