using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class CategoryADO : ICategory
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;

        public CategoryADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public Category AddCategory(Category category)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO Categories(CategoryName)
                VALUES(@CategoryName);
                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    conn.Open();
                    int CategoryId = Convert.ToInt32(cmd.ExecuteScalar());
                    category.CategoryId = CategoryId;
                    return category;
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

        public void DeleteCategory(int categoryId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM Categories WHERE CategoryId=@CategoryId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
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

        public IEnumerable<Category> GetCategories()
        {
            List<Category>categories = new List<Category>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Categories ORDER BY CategoryId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        Category category = new Category();
                        category.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        category.CategoryName = dr["CategoryName"].ToString();
                        categories.Add(category);

                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            Category category = new Category();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Categories WHERE CategoryId=@CategoryId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();
                    category.CategoryId=Convert.ToInt32(dr["CategoryId"]);
                    category.CategoryName=dr["CategoryName"].ToString();
                }
                else
                {
                    throw new Exception("Category not found");
                }
                return category;
            }
        }

        public Category UpdateCategory(Category category)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE Categories SET CategoryName=@CategoryName
                WHERE CategoryId=@CategoryId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Category not found");
                    }
                    return category;
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