using Microsoft.AspNetCore.Mvc;
using Sometimes.Database.Models;
using Sometimes.Models;
using Swashbuckle.AspNetCore.Annotations;
using Sometimes.Services.Interfaces;

namespace Sometimes.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly IUserInfoService UserInfoService;

    public UserInfoController(IUserInfoService userInfoService)
    {
        UserInfoService = userInfoService;
    }

    [HttpGet("")]
    [SwaggerOperation("GetUserInfo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfo))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserInfo([FromHeader] string uuid)
    {
        try
        {
            var user = await UserInfoService.GetUserInfo(uuid);

            if(user is null)
            {
                return NotFound();
            }
            else
            {
                return new OkObjectResult(user);
            }
        }
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("")]
    [SwaggerOperation("CreateUserInfo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfo))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserInfo([FromBody] UserInfo userInfo)
    {
        try
        {
            var user = await UserInfoService.CreateUserInfo(userInfo);

            if (user is null)
            {
                return BadRequest();
            }
            else
            {
                return new OkObjectResult(user);
            }
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("Friends")]
    [SwaggerOperation("GetFriends")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FriendInfo>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFriends([FromHeader] string uuid)
    {
        try
        {
            var friends = await UserInfoService.GetFriends(uuid);

            return new OkObjectResult(friends);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("AddFriend")]
    [SwaggerOperation("AddFriend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddFriend([FromHeader] string uuid, [FromHeader] string friendUuid)
    {
        try
        {
            var success = await UserInfoService.AddFriend(uuid, friendUuid);

            return success ? new OkResult() : StatusCode(500, "Unable to add friend");

        }
        catch (ArgumentException)
        {
            return BadRequest("One or both UUIDs were invalid");
        }
    }

    [HttpPost("RemoveFriend")]
    [SwaggerOperation("RemoveFriend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFriend([FromHeader] string uuid, [FromHeader] string friendUuid)
    {
        try
        {
            var success = await UserInfoService.RemoveFriend(uuid, friendUuid);

            return success ? new OkResult() : StatusCode(500, "Unable to add friend");

        }
        catch (ArgumentException)
        {
            return BadRequest("One or both UUIDs were invalid");
        }
    }


}

