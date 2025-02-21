using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Repository.Entities;

namespace HimariServer.Service.BusinessModels.BlogCategoryModels
{
    public class BlogCategoryModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        //public virtual ICollection<Blog> Blogs { get; set; }
    }
}
