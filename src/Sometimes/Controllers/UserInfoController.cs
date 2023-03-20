using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Sometimes.Database.Models;
using Sometimes.Services;

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
        this.UserInfoService = userInfoService;
    }

    [HttpGet(Name = "UserInfo")]
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
    public async Task<IActionResult> PutUserInfo([FromBody] UserInfo userInfo)
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

