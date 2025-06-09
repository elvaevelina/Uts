using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }

        public Category? Category { get; set; } // Navigation property to Category
        public IEnumerable<SaleItem> SaleItems { get; set; } = new List<SaleItem>(); // Navigation property to SaleItems
    }
}