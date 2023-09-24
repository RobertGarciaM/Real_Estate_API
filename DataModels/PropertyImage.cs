using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class PropertyImage
    {
        [Key]
        public Guid IdPropertyImage { get; set; }
        public byte[]? File { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
        public virtual Property? Property { get; set; }
    }
}