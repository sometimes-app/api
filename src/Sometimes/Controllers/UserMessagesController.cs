using Microsoft.AspNetCore.Mvc;
using Sometimes.Database.Models;
using Sometimes.Models;
using Swashbuckle.AspNetCore.Annotations;
using Sometimes.Services.Interfaces;

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

    [HttpGet("/messages/daily")]
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


    [HttpPut("/messages")]
    [SwaggerOperation("ReadMessage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReadMessage([FromHeader] string uuid, [FromHeader] string messageId)
    {
        try
        {
            var displayMessage = await UserMessagesService.ReadMessage(uuid, messageId);

            if (displayMessage)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}

