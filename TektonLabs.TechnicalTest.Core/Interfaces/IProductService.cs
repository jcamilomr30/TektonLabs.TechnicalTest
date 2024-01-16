using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Core.Interfaces
{
    public interface IProductService : IService<int, Product, Dtos.ProductDto>
    {
        Task<ProductDto> GetById(int id);
        Task<int> Create(CreateProductDto ProductDto);
        Task Update(UpdateProductDto ProductDto, int id);
    }
}
