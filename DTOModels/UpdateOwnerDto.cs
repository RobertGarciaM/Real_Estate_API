using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOModels
{
    public class UpdateOwnerDto
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }

        public IFormFile? Photo { get; set; }

        public DateTime Birthday { get; set; }
    }
}
