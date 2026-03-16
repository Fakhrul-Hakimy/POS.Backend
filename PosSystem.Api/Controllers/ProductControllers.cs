using Microsoft.AspNetCore.Mvc;
using PosSystem.Application.Interfaces;
using PosSystem.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PosSystem.Api.Controllers
{
    [ApiController]
    [Route("api/product")]

    [Authorize]

    public class ProductControllers : ControllerBase
    {
        private   readonly IProductService _productService;

        public ProductControllers(IProductService productService)
        {
            _productService=productService;
        }

        [HttpPost]
        public async Task<IActionResult> addProduct([FromBody]newProductDto dto)
        {
             var createdBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "System";

            var result =await _productService.AddNewProduct(dto, createdBy);

            if (result==false)
            {
                return BadRequest("Failed to create Product.");
            }
            else
            {
                return Ok(true);
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var product=await _productService.GetAllProduct();

            return Ok(product);
        }
    }
}