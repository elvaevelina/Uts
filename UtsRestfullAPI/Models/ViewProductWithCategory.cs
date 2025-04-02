using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.Models
{
    public class ViewProductWithCategory
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price {get;set;}
        public int StockQuantity {get;set;}
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }         

    }
}