using DTOModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Query.Owner
{
    public record GetPagedOwnersQuery : IRequest<IEnumerable<OwnerDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetPagedOwnersQuery(int page, int pageSize) {
            Page = page > 0 ? page : 1;
            PageSize = PageSize > 0 ? pageSize : 1;
        }
    }
}
