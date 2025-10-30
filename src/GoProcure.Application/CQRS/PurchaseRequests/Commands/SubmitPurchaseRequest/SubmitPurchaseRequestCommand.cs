using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.SubmitPurchaseRequest
{
    public sealed record SubmitPurchaseRequestCommand(Guid PurchaseRequestId) : IRequest<Unit>;

}
