using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.DTO
{
    public class SalessDTO
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }

        public CustomerDTO? Customer { get; set; } // Navigation property to Customer
        
        public List<SaleItemDTO>? Items { get; set; } // Daftar SaleItemDTO

    }
}