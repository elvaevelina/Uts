using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class ViewProductWithCategoryADO:IViewProductWithCategory
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;

        public ViewProductWithCategoryADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ViewProductWithCategory> GetViewProductWithCategories()
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM ViewProductWithCategory";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                List<ViewProductWithCategory> ViewProductWithCategories = new List<ViewProductWithCategory>();
                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while(dr.Read())
                    {
                        ViewProductWithCategory viewProductWithCategory = new ViewProductWithCategory();
                        viewProductWithCategory.ProductId = Convert.ToInt32(dr["ProductId"]);
                        viewProductWithCategory.ProductName = dr["ProductName"].ToString();                        
                        viewProductWithCategory.Price = Convert.ToInt32(dr["Price"]);
                        viewProductWithCategory.StockQuantity = Convert.ToInt32(dr["StockQuantity"]);
                        viewProductWithCategory.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        viewProductWithCategory.CategoryName = dr["CategoryName"].ToString();                        
                        ViewProductWithCategories.Add(viewProductWithCategory);
                    }
                    return ViewProductWithCategories;
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

        public ViewProductWithCategory GetViewProductWithCategoryById(int productId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM ViewProductWithCategory WHERE productId=@productId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@productId", productId);
                ViewProductWithCategory viewProductWithCategory = new ViewProductWithCategory();
                try 
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while(dr.Read())
                    {
                        viewProductWithCategory.ProductId = Convert.ToInt32(dr["ProductId"]);
                        viewProductWithCategory.ProductName = dr["ProductName"].ToString();                        
                        viewProductWithCategory.Price = Convert.ToInt32(dr["Price"]);
                        viewProductWithCategory.StockQuantity = Convert.ToInt32(dr["StockQuantity"]);
                        viewProductWithCategory.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        viewProductWithCategory.CategoryName = dr["CategoryName"].ToString();                        
                    }
                    return viewProductWithCategory;
                }
                catch(SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
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