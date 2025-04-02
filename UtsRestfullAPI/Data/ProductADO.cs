using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class ProductADO : IProduct
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;

        public ProductADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }
        
        public Product AddProduct(Product product)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO products(ProductName, CategoryId, Price, StockQuantity, Description)
                VALUES (@ProductName, @CategoryId, @Price, @StockQuantity, @Description);
                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                    cmd.Parameters.AddWithValue("@Description", product.Description);

                    conn.Open();
                    int ProductId = Convert.ToInt32(cmd.ExecuteScalar());
                    product.ProductId = ProductId;
                    return product;
                }
                catch(Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void DeleteProduct(int productId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public Product GetProductById(int productId)
        {
            Product product = new Product();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();
                    product.ProductId = Convert.ToInt32(dr["ProductId"]);
                    product.ProductName = dr["ProductName"].ToString();
                    product.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                    product.Price = Convert.ToDecimal(dr["Price"]);
                    product.StockQuantity = Convert.ToInt32(dr["StockQuantity"]);
                    product.Description = dr["Description"].ToString();
                }
                else
                {
                    throw new Exception("Product not found");
                }
                return product;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM products ORDER BY ProductId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        Product product = new Product();
                        product.ProductId = Convert.ToInt32(dr["ProductId"]);
                        product.ProductName = dr["ProductName"].ToString();
                        product.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        product.Price = Convert.ToDecimal(dr["Price"]);
                        product.StockQuantity = Convert.ToInt32(dr["StockQuantity"]);
                        product.Description = dr["Description"].ToString();
                        products.Add(product);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();   
            }
            return products;
        }

        public Product UpdateProduct(Product product)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE products SET
                ProductName = @ProductName,
                CategoryId = @CategoryId,
                Price = @Price,
                StockQuantity = @StockQuantity,
                Description = @Description
                WHERE ProductId = @ProductId";
                
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                    cmd.Parameters.AddWithValue("@Description", product.Description);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Product not found");
                    }
                    return product;
                }
                catch(Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }
    }
}