using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.CategoryModels
{
    public class AddCategoryModel
    {
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
