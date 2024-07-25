using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.BlogDTOs;
using MilkStore_BAL.ModelViews.ProductDTOs;
using Newtonsoft.Json;

namespace Client_MilkForKidsStore.Pages.BlogPage
{
    public class CreateBlogModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateBlogModel(HttpClient httpClient)
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

        public async Task<IActionResult> OnPostAsync([FromForm] BlogDtoRequest blogRequest, [FromForm] List<int> productIds)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blogProductDto = new BlogProductDto
            {
                BlogTitle = blogRequest.BlogTitle,
                BlogContent = blogRequest.BlogContent,
                BlogImage = blogRequest.BlogImage,
                Status = true, // Set status as needed
                productId = productIds
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7223/api/v1/Blog/createBlog", blogProductDto);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    TempData["Message"] = $"Blog created successfully! URL: {result.url}";
                }
                else
                {
                    TempData["Message"] = "Error creating blog.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error: {ex.Message}";
            }

            return RedirectToPage("/BlogPage/BlogIndex");
        }

    }
}
