﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _categoryService;
        public ProductCategoryController(IProductCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/categories")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategories();
            if (categories == null)
            {
                return NotFound("Categories not found");
            }
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound("No category match this id");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireAdminOrStaffRole")]
        [HttpPost()]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Category cannot null");
                }
                if (request.ProductCategoryName.IsNullOrEmpty())
                {
                    return BadRequest("Please fill all fields");
                }
                await _categoryService.CreateCategory(request);
                return Ok("Create category successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireAdminOrStaffRole")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto request, int id)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Category cannot null");
                }
                if (request.ProductCategoryName.IsNullOrEmpty())
                {
                    return BadRequest("Please fill all fields");
                }
                await _categoryService.UpdateCategory(id, request);
                return Ok("Update category successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategory(int id)
        //{
        //    try
        //    {
        //        await _categoryService.DeleteCategory(id);
        //        return Ok("Delete category successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //    }
        //}
    }
}
