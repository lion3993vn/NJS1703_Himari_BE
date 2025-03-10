namespace HimariServer.Service.BusinessModels.ProductSymptomModels
{
    public class CreateProductSymptomModel
    {
        public int? PartSymptomId { get; set; }
        public int? ProductId { get; set; }
    }

    public class CreateProductSymptomMutilModel
    {
        public int? ProductId { get; set; }
        public List<int>? ListPartSymptomId { get; set; }
    }
}
