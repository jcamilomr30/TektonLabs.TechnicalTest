using TektonLabs.TechnicalTest.Domain.Entities;
using TektonLabs.TechnicalTest.Domain.IRepository;
using Renting.ProjectName.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace TektonLabs.TechnicalTest.Domain.Repository
{
    public class ProductRepository : Repository<int, Product>, IProductRepository
    {
        private new readonly IQueryableUnitOfWork UnitOfWork;
        public ProductRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
