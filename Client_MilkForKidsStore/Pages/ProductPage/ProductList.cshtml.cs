using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.ProductDTOs;
using Newtonsoft.Json;

namespace Client_MilkForKidsStore.Pages.ProductPage
{
    public class ProductListModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ProductListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IList<ProductDtoResponse> Products { get; set; } = new List<ProductDtoResponse>();

        public async Task<IActionResult> OnGetAsync(string search)
        {
            if(!string.IsNullOrEmpty(search))
            {
                var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Product/search-product/{search}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<List<ProductDtoResponse>>(jsonResponse);
                }
                else
                {
                    Products = new List<ProductDtoResponse>();
                    TempData["Message"] = "Not found ";
                    return Page();
                }
                return Page();
            }
            else
            {
                var response = await _httpClient.GetAsync("https://localhost:7223/api/v1/Product/get-all-products");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<List<ProductDtoResponse>>(jsonResponse);
                }
                else
                {
                    Products = new List<ProductDtoResponse>();
                    TempData["Message"] = "Not found ";
                    return Page();
                }
                return Page();
            }
        }
    }
}
