using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.DTO
{
    public class SaleItemGetDTO
    {
        public int SaleItemId { get; set; }
        public int SaleId { get; set; } // Added SaleId for navigation
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ProductDTO? Product { get; set; } // Navigation property to Product
        
    }
}