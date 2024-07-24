using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.ProductDTOs;
using Newtonsoft.Json;

namespace Client_MilkForKidsStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IList<ProductDtoResponse> Products { get; set; } = new List<ProductDtoResponse>();

        public async Task<IActionResult> OnGetAsync()
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
                TempData["Message"] = "Not found customer.";
                return Page();
            }
            return Page();

        }
    }
}