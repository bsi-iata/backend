using Microsoft.SemanticKernel;

namespace GotoFreight.AICore.Services
{
    public interface IImageDetectionService
    {
        Task<ChatMessageContent> AddressCheckChatMessageContentAsync(string systemMessage, string userMessage);
        IAsyncEnumerable<StreamingChatMessageContent> AddressCheckStreamingChatMessageContentsAsync(string systemMessage, string userMessage);

        /// <summary>
        /// Detect objects through pictures.
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="systemMessage"></param>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        Task<ChatMessageContent> ImageDetectionChatMessageContentAsync(string imageUri, string systemMessage, string userMessage);

        /// <summary>
        /// Detect objects through pictures.
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="systemMessage"></param>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        IAsyncEnumerable<StreamingChatMessageContent> ImageDetectionStreamingChatMessageContentAsync(string imageUri, string systemMessage, string userMessage);
    }
}