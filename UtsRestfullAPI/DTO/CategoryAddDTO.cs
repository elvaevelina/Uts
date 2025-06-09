using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.DTO
{
    public class CategoryAddDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

    }
}