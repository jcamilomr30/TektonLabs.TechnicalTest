using System;
using System.Collections.Generic;
using System.Text;

namespace TektonLabs.TechnicalTest.Domain.Entities
{
    public class Product : EntityBase<int>
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
