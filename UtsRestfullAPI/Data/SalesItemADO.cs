using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class SalesItemADO : ISalesItem
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;
        
        public SalesItemADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = configuration.GetConnectionString("DefaultConnection");
        }

        public SaleItem AddSaleItem(SaleItem saleItem)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO SaleItems(SaleId, ProductId, Quantity, Price)
                VALUES (@SaleId, @ProductId, @Quantity, @Price);
                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@SaleId", saleItem.SaleId);
                    cmd.Parameters.AddWithValue("@ProductId", saleItem.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", saleItem.Quantity);
                    cmd.Parameters.AddWithValue("@Price", saleItem.Price);

                    conn.Open();
                    int SaleItemId = Convert.ToInt32(cmd.ExecuteScalar());
                    saleItem.SaleItemId = SaleItemId;
                    return saleItem;
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

        public void DeleteSaleItem(int saleItemId)
        {
           using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM SaleItems WHERE SaleItemId=@SaleItemId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@SaleItemId", saleItemId);
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

        public SaleItem GetSaleItemById(int saleItemId)
        {
          SaleItem saleItem = new SaleItem();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM SaleItems WHERE SaleItemId = @SaleItemId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@SaleItemId", saleItemId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();
                    saleItem.SaleItemId = Convert.ToInt32(dr["SaleItemId"]);
                    saleItem.SaleId = Convert.ToInt32(dr["SaleId"]);
                    saleItem.ProductId = Convert.ToInt32(dr["ProductId"]);
                    saleItem.Quantity = Convert.ToInt32(dr["Quantity"]);
                    saleItem.Price = Convert.ToDecimal(dr["Price"]);                        
                }
                else
                {
                    throw new Exception("Sale Item Not found");
                }
                return saleItem;
            }
        }

        public IEnumerable<SaleItem> GetSaleItems()
        {
            List<SaleItem> saleItems = new List<SaleItem>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM SaleItems ORDER BY SaleItemId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        SaleItem saleItem = new SaleItem();
                        saleItem.SaleItemId = Convert.ToInt32(dr["SaleItemId"]);
                        saleItem.SaleId = Convert.ToInt32(dr["SaleId"]);
                        saleItem.ProductId = Convert.ToInt32(dr["ProductId"]);
                        saleItem.Quantity = Convert.ToInt32(dr["Quantity"]);
                        saleItem.Price = Convert.ToDecimal(dr["Price"]);
                        saleItems.Add(saleItem);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return saleItems;
        }

        public SaleItem UpdateSaleItem(SaleItem saleItem)
        {
           using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE SaleItems SET
                SaleId=@SaleId,
                ProductId=@ProductId,
                Quantity=@Quantity,
                Price=@Price
                WHERE SaleItemId=@SaleItemId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@SaleItemId", saleItem.SaleItemId);
                    cmd.Parameters.AddWithValue("@SaleId", saleItem.SaleId);
                    cmd.Parameters.AddWithValue("@ProductId", saleItem.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", saleItem.Quantity);
                    cmd.Parameters.AddWithValue("@Price", saleItem.Price);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Product not found");
                    }
                    return saleItem;
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