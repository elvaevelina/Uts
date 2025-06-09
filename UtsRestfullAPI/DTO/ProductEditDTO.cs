using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.DTO
{
    public class ProductEditDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; } // Foreign key to Category

        public CategoryDTO? Category { get; set; } // Navigation property to Category

    }
}