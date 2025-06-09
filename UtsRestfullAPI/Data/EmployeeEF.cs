using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class EmployeeEF : IEmployee
    {
        private readonly ApplicationDbContext _context;
        public EmployeeEF(ApplicationDbContext context)
        {
            _context = context;
        }
        public Employee AddEmployee(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding employee", ex);
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            var employee = GetEmployeeById(employeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }
            try
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting employee", ex);
            }
        }

        public Employee GetEmployeeById(int employeeId)
        {
            // var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            var employee = (from e in _context.Employees
                            where e.EmployeeId == employeeId
                            select e).FirstOrDefault();
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }
            return employee;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            // var employees = _context.Employees.OrderBy(e => e.EmployeeName).ToList();
            var employees = from e in _context.Employees
                            orderby e.EmployeeName
                            select e;
            return employees;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var existingEmployee = GetEmployeeById(employee.EmployeeId);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employee.EmployeeId} not found.");
            }
            try
            {
                existingEmployee.EmployeeName = employee.EmployeeName;
                existingEmployee.ContactNumber = employee.ContactNumber;
                existingEmployee.Email = employee.Email;
                existingEmployee.Position = employee.Position;
                _context.SaveChanges();
                return existingEmployee;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating employee", ex);
            }
        }
    }
}