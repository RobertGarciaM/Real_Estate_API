using Microsoft.AspNetCore.Http;

namespace DTOModels
{
    public class CreatePropertyImageDto
    {
        public IFormFile? File { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
    }
}
