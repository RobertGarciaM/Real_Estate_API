namespace DTOModels
{
    public class PropertyTraceDto
    {
        public Guid Id { get; set; }
        public DateTime DateSale { get; set; }
        public string? Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public Guid IdProperty { get; set; }
    }
}