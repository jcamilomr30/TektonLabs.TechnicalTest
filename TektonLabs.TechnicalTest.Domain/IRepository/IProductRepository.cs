using TektonLabs.TechnicalTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TektonLabs.TechnicalTest.Domain.IRepository
{
    public interface IProductRepository : IRepository<int, Product>
    {
    }
}
