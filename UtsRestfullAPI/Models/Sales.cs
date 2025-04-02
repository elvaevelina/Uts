using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.Models
{
    public class Sales
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}