using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("/auth")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticatingDtoRequest loginInfo)
        {
            try
            {
                if (loginInfo == null) {
                    return BadRequest("Login information cannot be null");
                }
                if (loginInfo.UserName == null) { 
                    return BadRequest("Username cannot be empty");
                }
                if (loginInfo.Password == null) { 
                    return BadRequest("Password cannot be empty");
                }
                IActionResult response = Unauthorized();
                var isAuthenticated = await _accountService.AuthenticateUser(loginInfo);
                if (isAuthenticated != null) {
                    var accessToken = await _accountService.GenerateAccessToken(isAuthenticated);
                    if (accessToken.IsNullOrEmpty())
                    {
                        return BadRequest("Something went wrong");
                    }
                    response = Ok(new { accessToken = accessToken });
                    return response;
                }
                return NotFound("Wrong username or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
