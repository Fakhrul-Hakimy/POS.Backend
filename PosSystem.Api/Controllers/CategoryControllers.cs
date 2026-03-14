using Microsoft.AspNetCore.Mvc;
using PosSystem.Application.Interfaces;
using PosSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PosSystem.Api.Controllers
{
    [ApiController]
    [Route("api/category")]

    [Authorize]// Only Admin and Manager can access these endpoints
    public class CategoryControllers : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryControllers(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] newCategoryDto Dto)
        {
            var createdBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "System";

            bool success = await _categoryService.AddCategory(Dto, createdBy);
            if(success){
                return Ok(true);
            }else{
                return BadRequest("Failed to create category.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound("Category not found.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] updateCategoryDto Dto)
        {
             var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "System";

            bool success = await _categoryService.UpdateCategory(id, Dto, updatedBy);
            if (success)            {
                return Ok(true);
            }
            else
            {
                return NotFound("Category not found.");
            }
            

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
              var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "System";
            bool success = await _categoryService.DeleteCategory(id, updatedBy);
             if (success)            {
                return Ok(true);
            }
            else
            {
                return NotFound("Category not found.");
            }
        
        }
    }

}