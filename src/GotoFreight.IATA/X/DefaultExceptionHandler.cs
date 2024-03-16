using Microsoft.AspNetCore.Diagnostics;
using SJZY.Expand.ABP.Core.Common.Dto;

namespace GotoFreight.IATA.X;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DefaultExceptionHandler> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred");

        await httpContext.Response.WriteAsJsonAsync(new UnifyResultDto
            {
                Code = StatusCodes.Status500InternalServerError.ToString(),
                Msg = "Internal Server Error"
            }
        );

        return true;
    }
}