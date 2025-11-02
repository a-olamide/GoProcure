using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Queries.GetPurchaseRequestById
{
    public sealed record GetPurchaseRequestByIdQuery(Guid Id) : MediatR.IRequest<PurchaseRequestDto?>;

}
