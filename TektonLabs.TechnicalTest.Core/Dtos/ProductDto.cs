using System;
using System.Collections.Generic;
using System.Text;

namespace TektonLabs.TechnicalTest.Core.Dtos
{
    public class ProductDto : EntityBase<int>
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
