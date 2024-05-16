using Application.DTOs;
using Application.Interfaces.IServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using System.Linq.Expressions;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route(Common.Url.Product.GetAll)]
        public async Task<IActionResult> Index([FromQuery] ProductSearchDTO ProductSearch)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Product, bool>> filter = null;
            if (ProductSearch != null)
            {
                filter = item => item.Title.Contains(ProductSearch.Title);
            }
            return Ok(await _productService.GetWithFilter(filter, ProductSearch.PageIndex, ProductSearch.PageSize));
        }
    }
}
