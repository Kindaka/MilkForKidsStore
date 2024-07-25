using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.CartDTOs;
using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_DAL.Entities;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace Client_MilkForKidsStore.Pages.ProductPage
{
    public class ProductDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ProductDtoResponse? Product;
        public int Quantity;

        public ProductDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Product/get-product-by-id/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Product = JsonConvert.DeserializeObject<ProductDtoResponse>(jsonResponse);
            }
            else
            {
                Product = new ProductDtoResponse();
                TempData["Message"] = "Not found product.";
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int ProductId, int Quantity)
        {
            try
            {
                var customerId = GetCustomerId();
                var cartDtoRequest = new CartDtoRequest
                {
                    ProductId = ProductId,
                    CustomerId = customerId,
                    CartQuantity = Quantity,
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7223/api/v1/Cart", cartDtoRequest);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Add to cart success";
                }
                else
                {
                    TempData["ErrorMessage"] = "Add to cart fail";
                }
                return await OnGetAsync(ProductId);
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { errorMessage = ex.Message });
            }
        }

        public int GetCustomerId()
        {
            var accessToken = Request.Cookies["jsonToken"];
            if(string.IsNullOrEmpty(accessToken))
            {
                RedirectToPage("/Login");
            } else
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;
                var customerIdClaim = jsonToken?.Claims.FirstOrDefault(j => j.Type == "CustomerId");
                var customerId = int.Parse(customerIdClaim.Value);
                if (customerId == 0) RedirectToPage("/Error", new { errorMessage = "An error occurs with customer's account" });
                return customerId;
            }
            throw new Exception("Error");
        }
    }
}
