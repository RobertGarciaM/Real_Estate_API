using Microsoft.AspNetCore.Http;

namespace DTOModels
{
    public class UpdatePropertyImagesDto
    {
        public Guid PropertyImageId { get; set; }
        public IFormFile? File { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
    }
}
