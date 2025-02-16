using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.BlogModels
{
    public class UpdateBlogModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Status { get; set; }
    }
}
