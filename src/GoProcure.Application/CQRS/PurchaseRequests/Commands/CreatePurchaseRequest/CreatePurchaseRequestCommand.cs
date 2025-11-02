using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.CreatePurchaseRequest
{
    public sealed record CreatePurchaseRequestCommand(
    Guid RequesterId,
    string Department,
    string Currency = "USD"
) : IRequest<PurchaseRequestDto>;
}
