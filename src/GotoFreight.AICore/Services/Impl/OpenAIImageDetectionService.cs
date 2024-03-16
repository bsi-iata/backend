using GotoFreight.AICore.Extensions;
using GotoFreight.AICore.Helpers;
using GotoFreight.AICore.Models;
using GotoFreight.AICore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace GotoFreight.AICore.Services.Impl
{
    public class OpenAIImageDetectionService : IImageDetectionService
    {
        private readonly IConfiguration _configuration;
        private readonly OpenAIOptions _options;

        public OpenAIImageDetectionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _options = AIOptionHelper.BuildOpenAIOptions(configuration);
        }

        public IAsyncEnumerable<StreamingChatMessageContent> ImageDetectionStreamingChatMessageContentAsync(string imageUri, string systemMessage, string userMessage)
        {
            var imageContent = new ImageContent(new Uri(imageUri));

            var builder = Kernel.CreateBuilder();
            builder = builder.WithOpenAIVersion(_options);
            var kernel = builder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(systemMessage);

            chatHistory.AddUserMessage(new ChatMessageContentItemCollection
            {
                new TextContent(userMessage),
                imageContent
            });

            return chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, kernel: kernel, executionSettings: new OpenAIPromptExecutionSettings
            {
                MaxTokens = _options.VersionPreviewMaxTokens
            });
        }


        public Task<ChatMessageContent> ImageDetectionChatMessageContentAsync(string imageUri, string systemMessage, string userMessage)
        {
            var imageContent = new ImageContent(new Uri(imageUri));

            var builder = Kernel.CreateBuilder();
            builder = builder.WithOpenAIVersion(_options);
            var kernel = builder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(systemMessage);

            chatHistory.AddUserMessage(new ChatMessageContentItemCollection
            {
                new TextContent(userMessage),
                imageContent
            });

            return chatCompletionService.GetChatMessageContentAsync(chatHistory, kernel: kernel, executionSettings: new OpenAIPromptExecutionSettings
            {
                MaxTokens = _options.VersionPreviewMaxTokens
            });
        }


        public IAsyncEnumerable<StreamingChatMessageContent> AddressCheckStreamingChatMessageContentsAsync(string systemMessage, string userMessage)
        {
            var builder = Kernel.CreateBuilder();
            builder = builder.WithOpenAIChat(_options);
            var kernel = builder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(systemMessage);

            chatHistory.AddUserMessage(new ChatMessageContentItemCollection
            {
                new TextContent(userMessage)
            });

            return chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, kernel: kernel, executionSettings: new OpenAIPromptExecutionSettings
            {
                MaxTokens = _options.ChatCompletionMaxTokens
            });
        }

        public Task<ChatMessageContent> AddressCheckChatMessageContentAsync(string systemMessage, string userMessage)
        {
            var builder = Kernel.CreateBuilder();
            builder = builder.WithOpenAIChat(_options);
            var kernel = builder.Build();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage(systemMessage);

            chatHistory.AddUserMessage(new ChatMessageContentItemCollection
            {
                new TextContent(userMessage)
            });

            return chatCompletionService.GetChatMessageContentAsync(chatHistory, kernel: kernel, executionSettings: new OpenAIPromptExecutionSettings
            {
                MaxTokens = _options.ChatCompletionMaxTokens
            });
        }
    }
}
