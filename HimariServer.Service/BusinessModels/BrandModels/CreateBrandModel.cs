using HimariServer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.BrandModels
{
    public class CreateBrandModel
    {
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
