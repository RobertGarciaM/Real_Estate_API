﻿using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Query.Owner;
using RealEstate.Mediator.QueryHandlers.Owner;

namespace RealEstate.Mediator.Handlers.PropertyHandler
{
    internal class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreatePropertyCommandHandler(InMemoryDbContext repository, IMapper mapper, IMediator mediator)
        {
            _context = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ActionResult> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
         
            var ownerExists = await _mediator.Send(new CheckOwnerExistsQuery(request.Dto.IdOwner));

            if (!ownerExists)
            {
                return new NotFoundObjectResult(new { Message = "The Owner does not exists." });
            }

            var property = _mapper.Map<Property>(request.Dto);

            _ = await _context.AddAsync(property);
            _ = await _context.SaveChangesAsync();


            return new OkObjectResult(new { Id = property.IdProperty });
        }
    }
}
