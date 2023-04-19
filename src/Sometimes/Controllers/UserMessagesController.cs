using Microsoft.AspNetCore.Mvc;
using Sometimes.Database.Models;
using Sometimes.Models;
using Sometimes.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Sometimes.Controllers;

[ApiController]
[Route("[controller]")]

public class UserMessagesController : Controller
{
    private readonly IUserMessagesService UserMessagesService;
    public UserMessagesController(IUserMessagesService userMessagesService)
    {
        UserMessagesService = userMessagesService;
    }
    [HttpGet("")]
    [SwaggerOperation("GetDailyMessage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDailyMessage([FromHeader] string uuid)
    {
        try
        {
            var displayMessage = await UserMessagesService.GetDailyMessage(uuid);

            if (displayMessage is null)
            {
                return NotFound();
            }
            else
            {
                return new OkObjectResult(displayMessage);
            }
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}

