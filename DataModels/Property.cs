using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class Property
    {
        public int IdProperty { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }

        public Guid IdOwner { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }

        public virtual ICollection<PropertyTrace> PropertyTraces { get; set; }
    }
}