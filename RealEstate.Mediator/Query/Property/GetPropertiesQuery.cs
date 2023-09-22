using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Query.Property
{
    public class GetPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Year { get; set; }
        public string? CodeInternal { get; set; }
        public Guid IdOwner { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}