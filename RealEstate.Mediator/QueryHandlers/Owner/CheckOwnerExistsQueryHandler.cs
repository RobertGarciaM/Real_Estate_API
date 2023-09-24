using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.Owner;

namespace RealEstate.Mediator.QueryHandlers.Owner
{
    internal class CheckOwnerExistsQueryHandler : IRequestHandler<CheckOwnerExistsQuery, bool>
    {
        private readonly RealEstateDbContext _context;

        public CheckOwnerExistsQueryHandler(RealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckOwnerExistsQuery request, CancellationToken cancellationToken)
        {
            bool ownerExists = await _context.Owners.AnyAsync(o => o.IdOwner == request.OwnerId, cancellationToken);

            return ownerExists;
        }
    }
}
