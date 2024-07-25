using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.BlogDTOs;
using Newtonsoft.Json;

namespace Client_MilkForKidsStore.Pages.BlogPage
{
    public class BlogIndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public BlogIndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IList<BlogDtoResponse> Blogs { get; set; } = new List<BlogDtoResponse>();

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7223/api/v1/Blog/GetAllBlog");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Blogs = JsonConvert.DeserializeObject<List<BlogDtoResponse>>(jsonResponse);
            }
            else
            {
                Blogs = new List<BlogDtoResponse>();
                TempData["Message"] = "Not found customer.";
                return Page();
            }
            return Page();

        }
    }
}
