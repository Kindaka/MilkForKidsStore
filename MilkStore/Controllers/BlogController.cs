using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkStore_BAL.ModelViews.BlogDTOs;
using MilkStore_BAL.ModelViews.FeedbackDTOs;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_BAL.Services.Implements;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;

namespace MilkStore.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _service;

        public BlogController(IBlogService service)
        {
            _service = service;
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpGet("GetAllBlogByBlogId/{blogId}")]
        public async Task<IActionResult> GetAllBlogByBlogId(int blogId)
        {
            try
            {
                var getAll = await _service.GetAllBlogByBlogId(blogId);
                if (getAll == null)
                {
                    return Ok("Blog is empty !!");
                }

                return Ok(getAll);
            }
            catch
            {
                return BadRequest("Valid");
            }

        }

        [AllowAnonymous]
        [HttpGet("GetAllBlog")]
        public async Task<IActionResult> GetAllBlog()
        {
            try
            {
                var getAll = await _service.GetAllBlog();
                if (getAll.IsNullOrEmpty())
                {
                    return Ok("Blog is empty !!");
                }

                return Ok(getAll);
            }
            catch
            {
                return BadRequest("Valid");
            }

        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpPost("createBlog")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogProductDto blogItems)
        {
            try
            {
                var checkItem = await _service.ValidateProductOfBolg(blogItems);
                if (checkItem == -1)
                {
                    return BadRequest("Some items that not valid in your Product");
                }
                else
                {
                    var url = await _service.CreateBlog(blogItems);
                    return Ok(new { url = url });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
