using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.BlogDTOs;
using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_DAL.Entities;
using Newtonsoft.Json;
using System.Text.Json;

namespace Client_MilkForKidsStore.Pages.BlogPage
{
    public class BlogDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public BlogDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public BlogDtoResponse Blog { get; set; }

        public async Task<IActionResult> OnGetAsync(int blogId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Blog/GetAllBlogByBlogId/{blogId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Blog = JsonConvert.DeserializeObject<BlogDtoResponse>(jsonResponse);
            }
            else
            {
                TempData["Message"] = "Blog not found.";
                return RedirectToPage("/Error");
            }
            return Page();
        }
     }
}
