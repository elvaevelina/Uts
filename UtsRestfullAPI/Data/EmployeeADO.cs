using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class EmployeeADO : IEmployee
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;
        
        public EmployeeADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr=_configuration.GetConnectionString("DefaultConnection");
        }

        public Employee AddEmployee(Employee employee)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO Employees(EmployeeName, ContactNumber, Email, Position)
                VALUES (@EmployeeName, @ContactNumber, @Email, @Position);
                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    cmd.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Position", employee.Position);

                    conn.Open();
                    int employeeId = Convert.ToInt32(cmd.ExecuteScalar());
                    employee.EmployeeId = employeeId;
                    return employee;
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

        public void DeleteEmployee(int employeeId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM Employees WHERE EmployeeId=@EmployeeId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
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

        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = new Employee();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Employees WHERE EmployeeId=@EmployeeId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    dr.Read();              
                    employee.EmployeeId = Convert.ToInt32(dr["EmployeeId"]);
                    employee.EmployeeName = dr["EmployeeName"].ToString();
                    employee.ContactNumber = dr["ContactNumber"].ToString();
                    employee.Email = dr["Email"].ToString();
                    employee.Position = dr["Position"].ToString();
                    
                }
                else
                {
                    throw new Exception("Employees not found");
                }
                return employee;
            }
        }

        public IEnumerable<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Employees ORDER BY EmployeeId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while(dr.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(dr["EmployeeId"]);
                        employee.EmployeeName = dr["EmployeeName"].ToString();
                        employee.ContactNumber = dr["ContactNumber"].ToString();
                        employee.Email = dr["Email"].ToString();
                        employee.Position = dr["Position"].ToString();
                        employees.Add(employee);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return employees;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE Employees SET
                EmployeeName = @EmployeeName,
                ContactNumber = @ContactNumber,
                Email = @Email,
                Position = @Position
                WHERE EmployeeId = @EmployeeId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try 
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    cmd.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Position", employee.Position);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if(result == 0)
                    {
                        throw new Exception("Employee not found");
                    }
                    return employee;
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