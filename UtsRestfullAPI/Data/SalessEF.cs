using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class SalessEF : ISaless
    {
        private readonly ApplicationDbContext _context;
        public SalessEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public Saless AddSales(Saless sales)
        {
            try
            {
                _context.Saless.Add(sales);
                _context.SaveChanges();
                return sales;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding sales", ex);
            }
        }

        public void DeleteSales(int saleId)
        {
            var sale = GetSalesById(saleId);
            if (sale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
            }
            try
            {
                _context.Saless.Remove(sale);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting sales", ex);
            }
        }


        public IEnumerable<Saless> GetAllSalesWithDetails()
        {
            return _context.Saless
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p.Category)
                .OrderBy(s => s.SaleId)
                .ToList();
        }

        public Saless GetSaleDetail(int saleId)
        {
            return _context.Saless
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                    .ThenInclude(p => p.Category)
                .FirstOrDefault(s => s.SaleId == saleId);
        }

        public IEnumerable<Saless> GetSales()
        {

            // var sales = _context.Saless.OrderBy(s => s.SaleId).ToList();
            var sales = _context.Saless
                .Include(s => s.Customer)
                .ToList();
            return sales;
        }

        public Saless GetSalesById(int saleId)
        {
            // var sale = _context.Saless.FirstOrDefault(s => s.SaleId == saleId);
            var sale = _context.Saless.Include(s => s.Customer).FirstOrDefault(s => s.SaleId == saleId);
            if (sale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
            }
            return sale;
        }



        public Saless UpdateSales(Saless sales)
        {
            var existingSale = GetSalesById(sales.SaleId);
            if (existingSale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {sales.SaleId} not found.");
            }

            try
            {
                existingSale.CustomerId = sales.CustomerId;
                existingSale.SaleDate = sales.SaleDate;
                existingSale.TotalAmount = sales.TotalAmount;
                _context.Saless.Update(existingSale);
                _context.SaveChanges();
                return existingSale;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating sales", ex);
            }

            // var customerExists = _context.Customers.Any(c => c.CustomerId == sales.CustomerId);
            // if (!customerExists)
            // {
            //     throw new Exception($"CustomerId {sales.CustomerId} does not exist in Customers table.");
            // }

        }

    }
}