using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtsRestfullAPI.DTO
{
    public class ResetPasswordDTO
    {
        public string Username { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}