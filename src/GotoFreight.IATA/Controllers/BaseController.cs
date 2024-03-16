using Microsoft.AspNetCore.Mvc;

namespace GotoFreight.IATA.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    [NonAction]
    public long GetUserId()
    {
        var name = User.Identity.Name;
        return long.TryParse(name, out var userId) ? userId : 888;
    }
}