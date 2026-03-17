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
            if (product == null)
            {
                return BadRequest("No Data Found.");
            }
            else
            {
                return Ok(product);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return BadRequest("No Data Found.");
                }
                else
                {
                    return Ok(product);
                }
                
            }catch(Exception e)
            {
                return BadRequest("Error : "+e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct([FromBody] updateProductDto dto, int id)
        {
            try
            {
                
            
            var UpdateBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? "System";

            var result= await _productService.EditProduct(dto, id,UpdateBy);

            if (result==null)
            {
                return BadRequest("Failed to updated Product Details");
            }
            else
            {
                return Ok(true);
            }
            }catch(Exception e)
            {
                return BadRequest("Failed to update product details : "+e);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductId(int id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);

                if (result==true)
                {
                    return Ok("Product edit successfully");
                }
                else
                {
                    return BadRequest(false);
                }
                
            }catch(Exception e)
            {
                return BadRequest("Error : "+e);
            }
        }
    }
}