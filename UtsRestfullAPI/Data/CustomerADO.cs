using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class CustomerADO : ICustomer
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;
        
        public CustomerADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("DefaultConnection");
        }

        public Customer AddCustomer(Customer customer)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO Customers
                (CustomerName, ContactNumber, Email, Address)
                VALUES (@CustomerName, @ContactNumber, @Email, @Address);
                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                    cmd.Parameters.AddWithValue("@ContactNumber", customer.ContactNumber);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);

                    conn.Open();
                    int CustomerId = Convert.ToInt32(cmd.ExecuteScalar());
                    customer.CustomerId = CustomerId;
                    return customer;
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

        public void DeleteCustomer(int customerId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM Customers WHERE CustomerId=@CustomerId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
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

        public Customer GetCustomerById(int customerId)
        {
            Customer customer = new Customer();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();
                    customer.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                    customer.CustomerName = dr["CustomerName"].ToString();
                    customer.ContactNumber = dr["ContactNumber"].ToString();
                    customer.Email = dr["Email"].ToString();
                    customer.Address = dr["Address"].ToString();
                }
                else
                {
                    throw new Exception("Customer not found");
                }
                return customer;            
            }
        }

        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Customers ORDER BY CustomerId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        Customer customer = new Customer();
                        customer.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                        customer.CustomerName = dr["CustomerName"].ToString();
                        customer.ContactNumber = dr["ContactNumber"].ToString();
                        customer.Email = dr["Email"].ToString();
                        customer.Address = dr["Address"].ToString();
                        customers.Add(customer);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return customers;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE Customers SET
                CustomerName=@CustomerName,
                ContactNumber=@ContactNumber,
                Email=@Email, Address=@Address
                WHERE CustomerId=@CustomerId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                    cmd.Parameters.AddWithValue("@ContactNumber", customer.ContactNumber);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Product not found");
                    }
                    return customer;
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