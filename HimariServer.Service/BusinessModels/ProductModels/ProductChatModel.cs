using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ProductModels
{
    public class ProductChatModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Similarity { get; set; }
    }

    public class ProductEmbeddingModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string Symptomp { get; set; }
        public string BodyPart { get; set; }
    }
}
