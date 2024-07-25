using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.CustomerDTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

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
            if (CustomerId == 0 && User.Identity.IsAuthenticated)
            {
                var customerIdClaim = User.FindFirst("CustomerId")?.Value;
                if (string.IsNullOrEmpty(customerIdClaim))
                {
                    return BadRequest("Customer ID not found in claims.");
                }

                if (!int.TryParse(customerIdClaim, out var customerId))
                {
                    return BadRequest("Invalid Customer ID in claims.");
                }

                CustomerId = customerId;
            }

            var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Customer/{CustomerId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Customer = JsonConvert.DeserializeObject<CustomerDto>(jsonResponse) ?? new CustomerDto();
            }
            else
            {
                TempData["Message"] = "Customer not found.";
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (CustomerId == 0 && User.Identity.IsAuthenticated)
            {
                var customerIdClaim = User.FindFirst("CustomerId")?.Value;
                if (string.IsNullOrEmpty(customerIdClaim))
                {
                    return BadRequest("Customer ID not found in claims.");
                }

                if (!int.TryParse(customerIdClaim, out var customerId))
                {
                    return BadRequest("Invalid Customer ID in claims.");
                }

                CustomerId = customerId;
            }

            var updateRequest = new UpdateCustomerDto
            {
                UserName = UpdateCustomer.UserName,
                Phone = UpdateCustomer.Phone,
                Address = UpdateCustomer.Address,
                Dob = UpdateCustomer.Dob
            };

            var jsonContent = JsonConvert.SerializeObject(updateRequest);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7223/api/v1/Customer/{CustomerId}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Customer information updated successfully.";
                return RedirectToPage(new { CustomerId });
            }
            else
            {
                TempData["Message"] = "Error updating customer information.";
                return Page();
            }
        }
    }
}
