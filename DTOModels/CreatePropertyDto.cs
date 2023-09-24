using System.ComponentModel.DataAnnotations;

namespace DTOModels
{
    public class CreatePropertyDto
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Address { get; set; }
        public decimal Price { get; set; }
        [MaxLength(50)]
        public string? CodeInternal { get; set; }
        public int Year { get; set; }

        public Guid IdOwner { get; set; }
    }
}