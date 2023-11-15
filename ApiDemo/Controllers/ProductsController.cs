using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Sepecifcations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiDemo.HandelResponse;
using Services.Helper;
using Services.Services.ProductSerives;
using Services.Services.ProductSerives.Dto;
using ApiDemo.Helper;

namespace ApiDemo.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly IProductService _prouductService;

        // private readonly IProuductRepository _prouductRepository;

        public ProductsController (IProductService prouductService)
        {
            _prouductService = prouductService;
        }
        [HttpGet]
        [Cache(20)]
        public async Task<ActionResult<Pagination<ProductResultDto>>> GetProducts([FromQuery]ProductSepecifcations sepecifcations)
        //  => await _prouductService.GetProductsAsync( sepecifcations);
        {
            var prouducts = await _prouductService.GetProductsAsync(sepecifcations);

            return Ok(prouducts);

        }
        [Cache(500)]
        [HttpGet("{id}")]
        [ProducesResponseType (typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
       
        public async Task<ActionResult<ProductResultDto>> GetProductById(int? id)
        //=> await _prouductService.GetProductByIdAsync(id);
        {
            var product = await _prouductService.GetProductByIdAsync(id);

            if (product == null) 
                return NotFound(new ApiResponse (404));


            return Ok(product);
        }
       
        [HttpGet]
        [Route("Brands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _prouductService.GetProductBrandsAsync());

        [HttpGet("Types")]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
            => Ok(await _prouductService.GetProductTypesAsync());
    }
}
