using GotoFreight.AICore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GotoFreight.IATA.Controllers
{
    public class TestPost
    {
        public string ImageUrl { get; set; }

        public string SystemMessage { get; set; }
        public string UserMessage { get; set; }
    }

    public class TestController : BaseController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IImageDetectionService _aiService;

        public TestController(IHttpContextAccessor contextAccessor, IImageDetectionService aiService)
        {
            _contextAccessor = contextAccessor;
            _aiService = aiService;
        }

        [HttpPost("/testai")]
        public async Task TestAIAsync([FromBody] TestPost testPost)
        {
            var context = _contextAccessor.HttpContext;
            context.Response.Headers.ContentType = "text/event-stream";

            await foreach (var item in _aiService.ImageDetectionStreamingChatMessageContentAsync(testPost.ImageUrl, testPost.SystemMessage, testPost.UserMessage))
            {
                var data =
                    $"data:{item.Content}\n\n";

                // 将数据写入到响应流中
                await context.Response.WriteAsync(data, context.RequestAborted);
                await context.Response.Body.FlushAsync(context.RequestAborted);
            }
        }

        [HttpPost("/testai1")]
        public async Task<string> TestAI1Async([FromBody] TestPost testPost)
        {
            var result = await _aiService.ImageDetectionChatMessageContentAsync(testPost.ImageUrl, testPost.SystemMessage, testPost.UserMessage);
            return result.ToString();
        }
    }
}
