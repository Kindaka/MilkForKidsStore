using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_BAL.Services.Implements;
using MilkStore_BAL.Services.Interfaces;
using System.Text.RegularExpressions;

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
                if (loginInfo.Email == null) { 
                    return BadRequest("Email cannot be empty");
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
                return NotFound("Wrong email or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDtoRequest newAccount)
        {
            if (string.IsNullOrEmpty(newAccount.Email) || string.IsNullOrEmpty(newAccount.Password) || string.IsNullOrEmpty(newAccount.UserName))
            {
                return BadRequest("Please fill at least Username, email and password fields");
            }
            if (!IsValidEmail(newAccount.Email))
            {
                return BadRequest("Invalid email address");
            }
            if (!await _accountService.GetAccountByEmail(newAccount.Email))
            {
                bool checkRegister = await _accountService.CreateAccountCustomer(newAccount);
                if (checkRegister)
                {
                    return Ok("Create success");
                }
                else
                {
                    return BadRequest("Not correct role");
                }
            }
            else
            {
                return BadRequest("Existed email");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
