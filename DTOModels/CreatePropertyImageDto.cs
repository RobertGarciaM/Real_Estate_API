using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOModels
{
    public class CreatePropertyImageDto
    {
        public IFormFile File { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
    }
}
