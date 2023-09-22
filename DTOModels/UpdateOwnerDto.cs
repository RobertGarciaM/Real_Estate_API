using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOModels
{
    public class UpdateOwnerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public IFormFile Photo { get; set; }

        public DateTime Birthday { get; set; }
    }
}
