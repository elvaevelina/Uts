using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface ISalesItem
    {
        IEnumerable<SaleItem> GetSaleItems();
        SaleItem GetSaleItemById(int saleItemId);
        SaleItem AddSaleItem(SaleItem saleItem);
        SaleItem UpdateSaleItem(SaleItem saleItem);
        void DeleteSaleItem(int saleItemId);

    }
}