using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtsRestfullAPI.Helpers
{
    internal static class ApiSettings
    {
        internal static string SecretKey = "ZE7dSmG3uO4Vwg0rFt7iP14E26c0LWDC";
        internal static byte[] GenerateSecretByte() =>
            Encoding.ASCII.GetBytes(SecretKey);
    }  
}