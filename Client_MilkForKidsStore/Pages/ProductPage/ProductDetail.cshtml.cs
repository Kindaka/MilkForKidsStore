using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.CartDTOs;
using MilkStore_BAL.ModelViews.FeedbackDTOs;
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
        public IList<FeedbackDtoResponse> Feedback { get; set; } = new List<FeedbackDtoResponse>();


        public ProductDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Product/get-product-by-id/{id}");
            var responseFeedback = await _httpClient.GetAsync($"https://localhost:7223/api/Feedback/GetAllFeedback/{id}");

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
            if (responseFeedback.IsSuccessStatusCode)
            {
                var jsonResponseFb = await responseFeedback.Content.ReadAsStringAsync();

                if (jsonResponseFb.Contains("Feedback is empty"))
                {
                    Feedback = new List<FeedbackDtoResponse>();
                    TempData["Message"] = "No feedback available.";
                }
                else
                {
                    try
                    {
                        Feedback = JsonConvert.DeserializeObject<List<FeedbackDtoResponse>>(jsonResponseFb) ?? new List<FeedbackDtoResponse>();
                    }
                    catch (JsonSerializationException ex)
                    {
                        Feedback = new List<FeedbackDtoResponse>();
                        TempData["Message"] = "Error parsing feedback data: " + ex.Message;
                    }
                }
            }
            else
            {
                Feedback = new List<FeedbackDtoResponse>();
                TempData["Message"] = "Feedback not found.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int ProductId, int Quantity)
        {
            try
            {
                var jwtToken = Request.Cookies["jsonToken"];
                var customerId = GetCustomerId(jwtToken);
                var cartDtoRequest = new CartDtoRequest
                {
                    ProductId = ProductId,
                    CustomerId = customerId,
                    CartQuantity = Quantity,
                };
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

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
        public async Task<IActionResult> OnPostPostFeedbackAsync(int id, string feedbackContent)
        {
            feedbackContent = Request.Form["FeedbackContent"];

            if (string.IsNullOrWhiteSpace(feedbackContent))
            {
                TempData["ErrorMessage"] = "Feedback content cannot be empty.";
                return RedirectToPage("/ProductPage/ProductDetail", new { id = id });
            }

            try
            {
                var jwtToken = Request.Cookies["jsonToken"];
                var customerId = GetCustomerId(jwtToken);

                var feedbackDtoRequest = new FeedbackDtoRequest
                {
                    ProductId = id,
                    CustomerId = customerId,
                    FeedbackContent = feedbackContent,
                    RateNumber = 5
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7223/api/Feedback/CreateFeedback", feedbackDtoRequest);

                if (response.IsSuccessStatusCode)
                {
                    if (response != null)
                    {
                        TempData["SuccessMessage"] = "Feedback added successfully.";
                        return RedirectToPage("/ProductPage/ProductDetail", new { id = id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "You must purchase to receive feedback.";
                        return RedirectToPage("/ProductPage/ProductDetail", new { id = id });
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add feedback.";
                    return RedirectToPage("/ProductPage/ProductDetail", new { id = id });

                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Something wrong!!!";
                return RedirectToPage("/ProductPage/ProductDetail", new { id = id });

            }
        }

        public int GetCustomerId(string? accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                RedirectToPage("/AuthenticatePage/Login");
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