using System.ComponentModel.DataAnnotations;

namespace DTOModels
{
    public class CreatePropertyTraceDto
    {
        public DateTime DateSale { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public Guid IdProperty { get; set; }
    }
}
