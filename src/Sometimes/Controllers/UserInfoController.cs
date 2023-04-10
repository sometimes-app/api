using System;
using Microsoft.AspNetCore.Mvc;
using Sometimes.Database.Models;
using Sometimes.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Sometimes.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly ILogger<UserInfoController> logger;
    private readonly IUserInfoService UserInfoService;

    public UserInfoController(ILogger<UserInfoController> logger, IUserInfoService userInfoService)
    {
        this.logger = logger;
        UserInfoService = userInfoService;
    }

    [HttpGet(Name = "UserInfo")]
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

    [HttpPut(Name = "UserInfo")]
    [SwaggerOperation("AddUserInfo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfo))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserInfo([FromBody] UserInfo userInfo)
    {
        try
        {
            var user = await UserInfoService.PutUserInfo(userInfo);

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
}

