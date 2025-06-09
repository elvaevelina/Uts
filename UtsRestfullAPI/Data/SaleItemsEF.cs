using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class SaleItemsEF : ISalesItem
    {
        private readonly ApplicationDbContext _context;

        public SaleItemsEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public SaleItem AddSaleItem(SaleItem saleItem)
        {
            try
            {
                _context.SaleItems.Add(saleItem);
                _context.SaveChanges();
                return saleItem;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding SaleItem: " + ex.Message);
            }
            
        }

        public void DeleteSaleItem(int saleItemId)
        {
            var saleItem = GetSaleItemById(saleItemId);
            if (saleItem == null)
            {
                throw new KeyNotFoundException($"SaleItem with ID {saleItemId} not found.");
            }
            try
            {
                _context.SaleItems.Remove(saleItem);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting SaleItem", ex);
            }

        }

        public SaleItem GetSaleItemById(int saleItemId)
        {
            // var saleItem = _context.SaleItems.FirstOrDefault(s => s.SaleItemId == saleItemId);
            var saleItem = _context.SaleItems
            .Include(si => si.Product)
                .ThenInclude(p => p.Category)
            .Include(si => si.Saless)
            .FirstOrDefault(si => si.SaleItemId == saleItemId);
            if (saleItem == null)
            {
                throw new KeyNotFoundException($"SaleItem with ID {saleItemId} not found.");
            }
            return saleItem;
        }

        public IEnumerable<SaleItem> GetSaleItems()
        {
            // var saleItems = _context.SaleItems.OrderBy(si => si.SaleItemId).ToList();
            var saleItems = _context.SaleItems
                .Include(si => si.Product)
                    .ThenInclude(p => p.Category)
                .Include(si => si.Saless)
                .OrderBy(si => si.SaleItemId)
                .ToList();
            return saleItems;
        }

        public SaleItem UpdateSaleItem(SaleItem saleItem)
        {
            var existingSaleItem = GetSaleItemById(saleItem.SaleItemId);
            if (existingSaleItem == null)
            {
                throw new KeyNotFoundException($"SaleItem with ID {saleItem.SaleItemId} not found.");
            }

            try
            {
                existingSaleItem.SaleId = saleItem.SaleId;
                existingSaleItem.ProductId = saleItem.ProductId;
                existingSaleItem.Quantity = saleItem.Quantity;
                existingSaleItem.Price = saleItem.Price;
                _context.SaleItems.Update(existingSaleItem);
                _context.SaveChanges();
                return GetSaleItemById(existingSaleItem.SaleItemId);

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating SaleItem", ex);
            }
        }
    }
}