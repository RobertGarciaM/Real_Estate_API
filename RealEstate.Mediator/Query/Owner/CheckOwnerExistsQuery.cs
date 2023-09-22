using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Query.Owner
{
    public class CheckOwnerExistsQuery : IRequest<bool>
    {
        public Guid OwnerId { get; set; }

        public CheckOwnerExistsQuery(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
