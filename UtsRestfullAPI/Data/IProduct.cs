using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface IProduct
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int productId);
        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
        void DeleteProduct(int productId);
    }
}