using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface ISales
    {
        IEnumerable<Sales> GetSales();
        Sales GetSalesById(int saleId);
        Sales AddSales(Sales sales);
        Sales UpdateSales(Sales sales);
        void DeleteSales(int saleId);
    }
}