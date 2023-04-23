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
    [HttpGet("/message/daily")]
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
    [HttpPut("/message/read")]
    [SwaggerOperation("ReadDailyMessage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReadDailyMessage([FromHeader] string messageID)
    {
        try
        {
            var result = await UserMessagesService.ReadMessage(messageID);

            if (!result)
            {
                return NotFound();
            }
            else
            {
                return new OkResult();
            }
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut("/message/archive")]
    [SwaggerOperation("GetMessageArchive")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DisplayMessage>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageArchive([FromHeader] string uuid)
    {
        try
        {
            var result = await UserMessagesService.GetMessageArchive(uuid);

            if (result is null)
            {
                return NotFound();
            }
            else
            {
                return new OkObjectResult(result);
            }
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}

