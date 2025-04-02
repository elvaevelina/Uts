using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class ViewSalesProductCustomerOBADO : IViewSalesProductCustomerOB
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;

        public ViewSalesProductCustomerOBADO(IConfiguration configuration)        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ViewSalesProductCustomer> GetViewSalesProductCustomers()
        {
            List<ViewSalesProductCustomer> viewSalesProductCustomers = new List<ViewSalesProductCustomer>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM ViewSalesProductCustomer ORDER BY SaleId";
                SqlCommand cmd = new SqlCommand(strSql,conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        ViewSalesProductCustomer viewSalesProductCustomer = new ViewSalesProductCustomer();
                        viewSalesProductCustomer.SaleId = Convert.ToInt32(dr["SaleId"]);
                        viewSalesProductCustomer.SaleDate = Convert.ToDateTime(dr["SaleDate"]);
                        viewSalesProductCustomer.TotalAmount = Convert.ToInt32(dr["TotalAmount"]);
                        viewSalesProductCustomer.ProductId = Convert.ToInt32(dr["ProductId"]);
                        viewSalesProductCustomer.ProductName = dr["ProductName"].ToString();
                        viewSalesProductCustomer.Price = Convert.ToInt32(dr["Price"]);
                        viewSalesProductCustomer.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                        viewSalesProductCustomer.CustomerName = dr["CustomerName"].ToString();
                        viewSalesProductCustomers.Add(viewSalesProductCustomer);
                    }                
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return viewSalesProductCustomers;
        }
    }
}