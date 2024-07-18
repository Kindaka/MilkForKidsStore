using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_BAL.Services.Implements;
using MilkStore_BAL.Services.Interfaces;
using System.Security.Principal;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAccountService _accountService;

        public AdminController(IAdminService adminService, IAccountService accountService)
        {
            _adminService = adminService;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/accounts/staff")]
        public async Task<IActionResult> CreateAccountStaff([FromBody] UserRegisterDtoRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.UserName))
            {
                return BadRequest("Please fill at least Username, email and password fields");
            }
            if (!IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email address");
            }
            if (!await _accountService.GetAccountByEmail(request.Email))
            {
                bool checkCreated = await _adminService.CreateAccountStaff(request);
                if (checkCreated)
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

        [HttpPost("/api/v1/lock-account")]
        public async Task<IActionResult> LockAccount(int accountId)
        {
            bool check = await _adminService.LockAccount(accountId);
            if (check)
            {
                return Ok("Lock account success");
            } else
            {
                return BadRequest("Account not found");
            }
        }

        [HttpPost("/api/v1/unlock-account")]
        public async Task<IActionResult> UnlockAccount(int accountId)
        {
            bool check = await _adminService.UnlockAccount(accountId);
            if (check)
            {
                return Ok("Unlock account success");
            }
            else
            {
                return BadRequest("Account not found");
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
