using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface IViewSalesProductCustomerOB
    {
        IEnumerable<ViewSalesProductCustomer> GetViewSalesProductCustomers();
    }
}