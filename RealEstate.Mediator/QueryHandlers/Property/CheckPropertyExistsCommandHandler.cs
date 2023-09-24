using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyCommand;


namespace RealEstate.Mediator.QueryHandlers.PropertyHandler
{
    internal class CheckPropertyExistsCommandHandler : IRequestHandler<CheckPropertyExistsCommand, bool>
    {
        private readonly RealEstateDbContext _context;

        public CheckPropertyExistsCommandHandler(RealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckPropertyExistsCommand request, CancellationToken cancellationToken)
        {
            return await _context.Properties.AnyAsync(p => p.IdProperty == request.PropertyId, cancellationToken);
        }
    }
}
