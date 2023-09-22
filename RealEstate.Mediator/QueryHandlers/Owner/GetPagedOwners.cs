﻿using AutoMapper;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Query.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.QueryHandlers.Owner
{
    internal class GetPagedOwnersCommandHandler : IRequestHandler<GetPagedOwnersQuery, IEnumerable<OwnerDto>>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public GetPagedOwnersCommandHandler(InMemoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OwnerDto>> Handle(GetPagedOwnersQuery request, CancellationToken cancellationToken)
        {
            var owners = await _context.Owners
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }
    }
}
