using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Core.Interfaces;

namespace TektonLabs.TechnicalTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService ProductService;

        public ProductsController(IProductService ProductService)
        {
            this.ProductService = ProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var product = await ProductService.GetById(id);
            return Ok(product);
        }

        // POST: ProductsController/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto model)
        {
            var id = await ProductService.Create(model);
            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto model)
        {
            await ProductService.Update(model, id);

            return Ok();
        }
    }
}
