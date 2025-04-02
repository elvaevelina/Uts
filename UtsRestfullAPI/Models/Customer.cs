using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string ContactNumber{get; set;} = null!;
        public string Email{get;set;} = null!;
        public string Address{get;set;} = null!;
    }
}