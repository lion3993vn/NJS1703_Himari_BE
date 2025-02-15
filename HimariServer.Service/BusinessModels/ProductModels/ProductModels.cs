using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ProductModels
{
    public class ProductModels
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public string Description { get; set; }

        public int? Price { get; set; }

        public int? Quantity { get; set; }

        public string ImageUrl { get; set; }

        public string Status { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? BrandId { get; set; }

        public bool? Gender { get; set; }
    }
}
