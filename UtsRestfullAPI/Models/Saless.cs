using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.Models
{
    public class Saless
    {
        [Key]
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Customer? Customer { get; set; } // Navigation property to Customer
        public IEnumerable<SaleItem> SaleItems { get; set; } = new List<SaleItem>(); // Navigation property to SaleItems
    }
}