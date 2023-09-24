using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyTraceHandler
{
    internal class UpdatePropertyTraceCommandHandler : IRequestHandler<UpdatePropertyTraceCommand, ActionResult>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePropertyTraceCommandHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(UpdatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            DataModels.PropertyTrace? owner = await _context.PropertyTraces.FindAsync(request.dto.Id);
            if (owner == null)
            {
                return new NotFoundResult();
            }

            _ = _mapper.Map(request.dto, owner);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
