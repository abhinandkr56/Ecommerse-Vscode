using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entity;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseApiController
    {

        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;

            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;

        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProducttoReturnDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductsWithTypesandBrandsSpecification(productSpecParams);
             var countspec=new ProductwithFiltersForCountSpecification(productSpecParams);
            var totalItems = await _productRepo.CountAsync(countspec);
            var products = await _productRepo.LisAsync(spec);
            var data=_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProducttoReturnDto>>(products);
            return Ok(new Pagination<ProducttoReturnDto>(productSpecParams.PageIndex, productSpecParams.PageSize, totalItems, data));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProducttoReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesandBrandsSpecification(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            if(product==null)
            {
                return NotFound(new ApiResponse(404));
            }
            return _mapper.Map<Product,ProducttoReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var ProductBrand = await _productBrandRepo.ListAllAsync();
            return Ok(ProductBrand);

        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            var ProductType = await _productTypeRepo.ListAllAsync();
            return Ok(ProductType);
        }
    }
}