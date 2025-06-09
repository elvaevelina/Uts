using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UtsRestfullAPI.Models;
// using Microsoft.EntityFrameworkCore;

namespace UtsRestfullAPI.Data
{
    public class ProductEF : IProduct
    {
        private readonly ApplicationDbContext _context;
        public ProductEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product", ex);
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = GetProductById(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product", ex);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = from p in _context.Products.Include(p => p.Category)
                           orderby p.ProductName
                           select p;
            return products;
        }

        public Product GetProductById(int productId)
        {
            var product = _context.Products
                .Include(p => p.Category) // Include the Category navigation property
                .FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = _context.Products
                .Include(p => p.Category) // Include the Category navigation property
                .OrderBy(p => p.ProductName)
                .ToList();
            // var products = _context.Products.OrderBy(p => p.ProductName).ToList();
            // var products = from p in _context.Products
            //     orderby p.ProductName
            //     select p;
            return products;
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Product UpdateProduct(Product product)
        {
            var existingProduct = GetProductById(product.ProductId);
                if (existingProduct == null)
                {
                    throw new Exception("Product not found.");
                }
                try
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.Price = product.Price;
                    existingProduct.StockQuantity = product.StockQuantity;
                    existingProduct.Description = product.Description;
                    _context.Products.Update(existingProduct);
                    _context.SaveChanges();
                    return existingProduct;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating product", ex);
                }
        }
    }
}