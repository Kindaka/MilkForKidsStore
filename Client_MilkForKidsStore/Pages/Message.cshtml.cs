using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore_BAL.ModelViews.ChatDTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace Client_MilkForKidsStore.Pages
{
    public class MessageModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MessageModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            MessageHistory = new List<MessageDtoResponse>(); // Initialize MessageHistory
        }

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        public IList<MessageDtoResponse> MessageHistory { get; set; }
        public string CustomerName { get; set; }
        [BindProperty]
        public string Content { get; set; }

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

            var response = await _httpClient.GetAsync($"https://localhost:7223/api/v1/Message/history/{CustomerId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var chatHistoryResponse = JsonConvert.DeserializeObject<ChatHistoryResponse>(jsonResponse);
                MessageHistory = chatHistoryResponse.MessageHistory ?? new List<MessageDtoResponse>();
                CustomerName = chatHistoryResponse.CustomerName;
                System.Diagnostics.Debug.WriteLine("Chat History: " + JsonConvert.SerializeObject(MessageHistory));
            }
            else
            {
                MessageHistory = new List<MessageDtoResponse>();
                TempData["Message"] = "Chat history not found.";
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

            var messageRequest = new MessageDtoRequest
            {
                CustomerId = CustomerId,
                Content = Content,
                SendTime = DateTime.Now
            };

            var jsonContent = JsonConvert.SerializeObject(messageRequest);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7223/api/v1/Message", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage(new { CustomerId });
            }
            else
            {
                TempData["Message"] = "Error sending message.";
                return Page();
            }
        }
    }
}
