using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Query.PropertyCommand
{
    public record CheckPropertyExistsCommand : IRequest<bool>
    {
        public Guid PropertyId { get; set; }

        public CheckPropertyExistsCommand(Guid propertyId)
        {
            PropertyId = propertyId;
        }
    }
}
