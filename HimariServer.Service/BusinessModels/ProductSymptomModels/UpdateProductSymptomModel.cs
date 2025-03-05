namespace HimariServer.Service.BusinessModels.ProductSymptomModels
{
    public class UpdateProductSymptomModel
    {
        public int Id { get; set; }
        public int? PartSymptomId { get; set; }
        public int? ProductId { get; set; }
    }
}
