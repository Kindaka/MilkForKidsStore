using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkStore_BAL.ModelViews.FeedbackDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpGet("GetAllFeedback/{productId}")]
        public async Task<IActionResult> GetAllFeedbackOfProduct(int productId)
        {
            try
            {
                var getAll = await _service.GetAllFeedbackOfProduct(productId);
                if (getAll.IsNullOrEmpty())
                {
                    return Ok("Feedback is empty !!");
                }

                return Ok(getAll);
            }
            catch
            {
                return BadRequest("Valid");
            }

        }
        [HttpGet("GetRate/{productId}")]
        public async Task<IActionResult> GetRatingShop(int productId)
        {
            try
            {
                var getOneRating = await _service.GetRatingProduct(productId);
                if (getOneRating == null)
                {
                    return NotFound();
                }
                return Ok(getOneRating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOneFeedback/{productId}/{accountId}")]
        public async Task<IActionResult> GetOneFb(int productId, int accountId)
        {
            try
            {
                var getOne = await _service.GetOneFb(productId, accountId);
                if (getOne == null)
                {
                    return Ok("Feedback not found !!");
                }

                return Ok(getOne);
            }
            catch
            {
                return BadRequest("Valid");
            }
        }

        [HttpPost("CreateFeedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackDtoRequest request)
        {
            try
            {
                var response = await _service.CreateFeedback(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateFeedback/{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, [FromBody] FeedbackDtoRequest request)
        {
            try
            {
                var response = await _service.UpdateFeedback(feedbackId, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateStsAdmin/{feedbackId}")]
        public async Task<IActionResult> UpdateStsAdmin(int feedbackId, [FromBody] UpdateFeedbackDtoRequest request)
        {
            try
            {
                var response = await _service.UpdateStsAdmin(feedbackId, request);
                if (response)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{feedbackId}/{accountId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId, int accountId)
        {
            try
            {
                var response = await _service.DeleteFeedback(feedbackId, accountId);
                if (response)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
