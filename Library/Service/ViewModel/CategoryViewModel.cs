using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel
{
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public List<LogCategory> LogCategories { get; set; }
        public Category NewCategory { get; set; }
    }
}
