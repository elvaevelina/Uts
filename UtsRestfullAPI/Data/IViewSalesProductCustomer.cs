using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface IViewSalesProductCustomer
    {
        IEnumerable<ViewSalesProductCustomer> GetViewSalesProductCustomers();
        ViewSalesProductCustomer GetViewSalesProductCustomerById(int saleId);
    }
}