using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.AccountDTOs;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Nodes;
using System.Text;

namespace Client_MilkForKidsStore.Pages.AuthenticatePage
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public UserRegisterDtoRequest InfoView { get; set; }

        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public RegisterModel(HttpClient httpClient, IConfiguration config, ILogger<IndexModel> logger)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (InfoView.Password != InfoView.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }
            var content = new StringContent(JsonConvert.SerializeObject(InfoView), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7223/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/AuthenticatePage/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                return Page();
            }
            
        }
    }
}
