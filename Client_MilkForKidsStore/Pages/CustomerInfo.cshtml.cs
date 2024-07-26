using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.CustomerDTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Client_MilkForKidsStore.Pages
{
    public class CustomerInfoModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CustomerInfoModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        public CustomerDto Customer { get; set; } = new CustomerDto();

        [BindProperty]
        public UpdateCustomerDto UpdateCustomer { get; set; } = new UpdateCustomerDto();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var jwtToken = Request.Cookies["jsonToken"];
                if (string.IsNullOrEmpty(jwtToken))
                {
                    return RedirectToPage("/AuthenticatePage/Login");
                }

                var customerId = GetCustomerId(jwtToken);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

                var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Customer/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Customer = JsonConvert.DeserializeObject<CustomerDto>(jsonResponse) ?? new CustomerDto();
                    UpdateCustomer = new UpdateCustomerDto
                    {
                        UserName = Customer.UserName,
                        Phone = Customer.Phone,
                        Address = Customer.Address,
                        Dob = Customer.Dob
                    };
                }
                else
                {
                    TempData["Message"] = "Customer not found.";
                    return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var jwtToken = Request.Cookies["jsonToken"];
                if (string.IsNullOrEmpty(jwtToken))
                {
                    return RedirectToPage("/AuthenticatePage/Login");
                }

                var customerId = GetCustomerId(jwtToken);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

                var jsonContent = JsonConvert.SerializeObject(UpdateCustomer);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"https://localhost:7223/api/v1/Customer/{customerId}", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Customer information updated successfully.";
                    return RedirectToPage(new { CustomerId = customerId });
                }
                else
                {
                    TempData["Message"] = "Error updating customer information.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
                return Page();
            }
        }

        private int GetCustomerId(string? accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Access token is null or empty.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;
            var customerIdClaim = jsonToken?.Claims.FirstOrDefault(j => j.Type == "CustomerId");
            var customerId = int.Parse(customerIdClaim.Value);
            return customerId;
        }
    }
}
