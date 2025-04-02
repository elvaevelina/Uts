using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;


namespace UtsRestfullAPI.Data
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetEmployees();
        Employee GetEmployeeById(int employeeId);
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        void DeleteEmployee(int employeeId);
    }
}