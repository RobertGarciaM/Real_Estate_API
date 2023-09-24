using DTOModels;
using MediatR;

namespace RealEstate.Mediator.Query.Owner
{
    public record GetPagedOwnersQuery : IRequest<IEnumerable<OwnerDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetPagedOwnersQuery(int page, int pageSize)
        {
            Page = page > 0 ? page : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
        }
    }
}
