using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyCommand;


namespace RealEstate.Mediator.QueryHandlers.PropertyHandler
{
    internal class CheckPropertyExistsCommandHandler : IRequestHandler<CheckPropertyExistsCommand, bool>
    {
        private readonly InMemoryDbContext _context;

        public CheckPropertyExistsCommandHandler(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckPropertyExistsCommand request, CancellationToken cancellationToken)
        {
            return await _context.Properties.AnyAsync(p => p.IdProperty == request.PropertyId, cancellationToken);
        }
    }
}
