using DTOModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.QueryHandlers.Owner
{
    public record GetPagedOwnersCommand : IRequest<IEnumerable<OwnerDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
