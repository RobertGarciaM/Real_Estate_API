﻿using DTOModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Query.PropertyTrace
{
    public record GetPropertyTracesByPropertyIdQuery : IRequest<IEnumerable<PropertyTraceDto>>
    {
        public Guid PropertyId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetPropertyTracesByPropertyIdQuery(Guid propertyId, int page, int pageSize)
        {
            PropertyId = propertyId;
            Page = page > 0 ? page : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
        }
    }
}