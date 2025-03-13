using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.BlogModels
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public string Content { get; set; }

        public int? UserId { get; set; }
        public int BlogCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string FullName { get; set; }
    }
}
