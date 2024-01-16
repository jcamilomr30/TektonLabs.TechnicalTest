using System;
using System.Collections.Generic;
using System.Text;

namespace TektonLabs.TechnicalTest.Core.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
