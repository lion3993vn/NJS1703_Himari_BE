using System;

namespace HimariServer.Service.BusinessModels.ProductSymptomModels
{
    public class ProductSymptomModel
    {
        public int Id { get; set; }
        public int? PartSymptomId { get; set; }
        public int? ProductId { get; set; }
        public string PartSymptomName { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
