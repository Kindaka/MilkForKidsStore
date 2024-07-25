using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.CustomerDTOs;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_DAL.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Client_MilkForKidsStore.Pages
{
    public class OrdersCustomerModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public OrdersCustomerModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }
        public IList<OrderDtoResponse> Orders { get; set; } = new List<OrderDtoResponse>();
        public async Task<IActionResult> OnGetAsync(int status)
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
            var token = Request.Cookies["jsonToken"];




            
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.GetAsync($"https://localhost:7223/api/Order/customerId={CustomerId}&status={status}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Orders = JsonConvert.DeserializeObject<List<OrderDtoResponse>>(jsonResponse);
            }
            else
            {
                Orders = new List<OrderDtoResponse>();
                TempData["Message"] = "Orders not found.";
                return Page();
            }

            return Page();
        }
    }
}
