using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface IViewProductWithCategory
    {
        IEnumerable<ViewProductWithCategory> GetViewProductWithCategories();
        ViewProductWithCategory GetViewProductWithCategoryById(int productId);
    }
}