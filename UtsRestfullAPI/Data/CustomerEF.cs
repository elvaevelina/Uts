using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class CustomerEF : ICustomer
    {
        private readonly ApplicationDbContext _context;
        public CustomerEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer AddCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding customer", ex);
            }
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = GetCustomerById(customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }
            try
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting customer", ex);
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            // var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            var customer = (from c in _context.Customers
                            where c.CustomerId == customerId
                            select c).FirstOrDefault();
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }
            return customer;
        }

        public object GetCustomerById(object customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            // var customers = _context.Customers.OrderBy(c => c.CustomerName).ToList();
            var customers = from c in _context.Customers
                            orderby c.CustomerName
                            select c;
            return customers;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            var existingCustomer = GetCustomerById(customer.CustomerId);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customer.CustomerId} not found.");
            }

            try
            {
                existingCustomer.CustomerName = customer.CustomerName;
                existingCustomer.ContactNumber = customer.ContactNumber;
                existingCustomer.Email = customer.Email;
                existingCustomer.Address = customer.Address;
                _context.SaveChanges();
                return existingCustomer;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer", ex);
            }
        }
    }
}