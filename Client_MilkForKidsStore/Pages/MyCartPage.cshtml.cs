using Client_MilkForKidsStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.CartDTOs;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_DAL.Entities;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace Client_MilkForKidsStore.Pages
{
    public class MyCartPageModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<CartDtoResponse>? Carts;

        public MyCartPageModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var customerId = GetCustomerId();
                var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Cart/{customerId}");
                if(response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Carts = JsonConvert.DeserializeObject<List<CartDtoResponse>>(jsonResponse);
                    if(Carts != null && Carts.Any())
                    {
                        return Page();
                    }
                    else
                    {
                        TempData["Message"] = "Empty Cart";
                        Carts = new List<CartDtoResponse>();
                        return Page();
                    }
                } else
                {
                    return RedirectToPage("/Error", new { errorMessage = "Not found cart" });
                }
            } catch(Exception ex)
            {
                return RedirectToPage("/Error", new { errorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var customerId = GetCustomerId();
                var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Cart/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Carts = JsonConvert.DeserializeObject<List<CartDtoResponse>>(jsonResponse);

                    if(Carts != null && Carts.Any())
                    {
                        var orderProducts = Carts.Select(item => new OrderProductDto
                        {
                            cartId = item.CartId,
                            customerId = customerId,
                            productId = item.ProductView.ProductId,
                            quantity = item.CartQuantity
                        }).ToList();
                        var checkOutResponse = await _httpClient.PostAsJsonAsync("https://localhost:7223/api/Order/createOrder", orderProducts);
                        if (checkOutResponse.IsSuccessStatusCode)
                        {
                            var checkOutResponseContent = await checkOutResponse.Content.ReadAsStringAsync();
                            var checkOutResult = JsonConvert.DeserializeObject<CheckoutResponse>(checkOutResponseContent);

                            if (!string.IsNullOrEmpty(checkOutResult.url))
                            {
                                return Redirect(checkOutResult.url);
                            }
                            else
                            {
                                TempData["Message"] = "Checkout successful, but no URL was returned.";
                                return Page();
                            }
                        }
                        else
                        {
                            TempData["Message"] = "An error occurred during checkout.";
                            return Page();
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Empty Cart";
                        return Page();
                    }

                }
                else
                {
                    return RedirectToPage("/Error", new { errorMessage = "Error when call" });
                }
                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error", new { errorMessage = ex.Message });
            }
        }

        public int GetCustomerId()
        {
            var accessToken = Request.Cookies["jsonToken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                RedirectToPage("/Login");
            }
            else
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
