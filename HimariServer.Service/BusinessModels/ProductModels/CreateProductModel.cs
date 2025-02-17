using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ProductModels
{
    public class CreateProductModel
    {
        public required string ProductName { get; set; }

        public required string Description { get; set; }

        public int? Price { get; set; }

        public int? Quantity { get; set; }

        public required string ImageUrl { get; set; }

        public required string Status { get; set; }

        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        public bool? Gender { get; set; }
    }
}
