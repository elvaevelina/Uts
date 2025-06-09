using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.DTO
{
    public class SaleItemAddDTO
    {
        public int SaleItemId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }        

        public SalessGetDTO? Saless { get; set; } // Navigation property to Sale
        public ProductDTO? Product { get; set; } // Navigation property to Product


    }
}