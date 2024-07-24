using Client_MilkForKidsStore.Pages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.AccountDTOs;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;

namespace Client_MilkForKidsStore.AuthenticatePage
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserAuthenticatingDtoRequest InfoView { get; set; }

        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public LoginModel(HttpClient httpClient, IConfiguration config, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            //var loginData = new { Email, Password };
            var content = new StringContent(JsonConvert.SerializeObject(InfoView), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7223/auth", content);

            if (response.IsSuccessStatusCode)
            {
                var accessToken = JsonNode.Parse(await response.Content.ReadAsStringAsync())["accessToken"].ToString();

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(accessToken);
                if (jsonToken is JwtSecurityToken tokenS)
                {
                    var roleClaim = tokenS.Claims.FirstOrDefault(x => x.Type == "RoleId");
                    if (roleClaim != null)
                    {
                        var role = int.Parse(roleClaim.Value);

                        

                        if (role == 3)
                        {
                            Response.Cookies.Append("jsonToken", accessToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                Expires = DateTimeOffset.UtcNow.AddHours(1)
                            });
                            return RedirectToPage("/Index");
                        }

                    }
                    else
                    {
                        _logger.LogError("Role claim not found in the token.");
                        ViewData["ErrorMessage"] = "Role claim not found in the token.";
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        public async Task<IActionResult> OnPostLogout()
        {
            
            Response.Cookies.Delete("jsonToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/"
            });
            return RedirectToPage("/Index");
        }

    }
}
