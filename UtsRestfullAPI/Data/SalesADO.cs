using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class SalesADO : ISales
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;
        
        public SalesADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public Sales AddSales(Sales sales)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO Saless
                (CustomerId, SaleDate, TotalAMount)
                VALUES(@CustomerId, @SaleDate, @TotalAmount);
                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql,conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CustomerId", sales.CustomerId);
                    cmd.Parameters.AddWithValue("@SaleDate", sales.SalesDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", sales.TotalAmount);

                    conn.Open();
                    int saleId = Convert.ToInt32(cmd.ExecuteScalar());
                    sales.SaleId = saleId;
                    return sales;
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

        public void DeleteSales(int saleId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM Saless WHERE SaleId=@SaleId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@SaleId", saleId);
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

        public IEnumerable<Sales> GetSales()
        {                
            List<Sales> saless = new List<Sales>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Saless ORDER BY SaleId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        Sales sales = new Sales();
                        sales.SaleId = Convert.ToInt32(dr["SaleId"]);
                        sales.CustomerId= Convert.ToInt32(dr["CustomerId"]);
                        sales.SalesDate = Convert.ToDateTime(dr["SaleDate"]);
                        sales.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                        saless.Add(sales);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return saless;
        }

        public Sales GetSalesById(int saleId)
        {
            Sales sales = new Sales();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Saless WHERE SaleId = @SaleId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@SaleId", saleId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();
                    sales.SaleId = Convert.ToInt32(dr["SaleId"]);
                    sales.CustomerId= Convert.ToInt32(dr["CustomerId"]);
                    sales.SalesDate = Convert.ToDateTime(dr["SaleDate"]);
                    sales.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                }
                else
                {
                    throw new Exception("Sale not found");
                }
                return sales;
            }             
       }

        public Sales UpdateSales(Sales sales)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE Saless SET
                CustomerId = @CustomerId,
                SaleDate = @SaleDate,
                TotalAmount = @TotalAmount
                WHERE SaleId = @SaleId";
                SqlCommand cmd = new SqlCommand(strSql,conn);
                try
                {
                    cmd.Parameters.AddWithValue("@SaleId", sales.SaleId);
                    cmd.Parameters.AddWithValue("@CustomerId", sales.CustomerId);
                    cmd.Parameters.AddWithValue("@SaleDate", sales.SalesDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", sales.TotalAmount);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Sale not found");
                    }
                    return sales;
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