using GotoFreight.AICore.Services;
using GotoFreight.IATA.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GotoFreight.IATA.Controllers
{
    public class AddressController : BaseController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IImageDetectionService _aiService;

        public AddressController(IHttpContextAccessor contextAccessor, IImageDetectionService aiService)
        {
            _contextAccessor = contextAccessor;
            _aiService = aiService;
        }

        /// <summary>
        /// check address
        /// </summary>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        [HttpPost("check_address")]
        public async Task<AddressResult> CheckAddressAsync([FromBody] AddressDto addressDto)
        {
            var userMessage = 
                $"""
                Please determine whether the address is suspected to be invalid. Only answer is valid or suspected to be invalid. Please reply in English：{addressDto.Address}
                """;
            AddressResult result = new();
            var chatResult = await _aiService.AddressCheckChatMessageContentAsync("You are a map address checking system that can determine whether the address information entered is correct.", userMessage);
            result.Result = chatResult.ToString();
            return result;
        }
    }
}
