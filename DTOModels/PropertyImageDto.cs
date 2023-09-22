using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOModels
{
    public class PropertyImageDto
    {
        public Guid Id { get; set; }
        public byte[] File { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
    }
}
