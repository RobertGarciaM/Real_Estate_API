﻿using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class PropertyTrace
    {
        [Key]
        public Guid IdPropertyTrace { get; set; }
        public DateTime DateSale { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public Guid IdProperty { get; set; }
        public virtual Property? Property { get; set; }
    }
}