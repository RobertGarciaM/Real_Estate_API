using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class Owner
    {
        [Key]
        public Guid IdOwner { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        public byte[]? Photo { get; set; }

        public DateTime Birthday { get; set; }

        public virtual ICollection<Property>? Properties { get; set; }
    }
}