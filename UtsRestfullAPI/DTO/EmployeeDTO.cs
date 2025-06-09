using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.DTO
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
    }
}