using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface ISaless
    {
        IEnumerable<Saless> GetSales();
        Saless GetSalesById(int saleId);
        Saless AddSales(Saless sales);
        Saless UpdateSales(Saless sales);
        void DeleteSales(int saleId);

        Saless GetSaleDetail(int saleId);
        IEnumerable<Saless> GetAllSalesWithDetails();

    }
}