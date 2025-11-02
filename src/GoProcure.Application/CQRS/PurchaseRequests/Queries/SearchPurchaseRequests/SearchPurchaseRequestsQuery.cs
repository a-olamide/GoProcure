using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Queries.SearchPurchaseRequests
{
    public sealed record SearchPurchaseRequestsQuery(string? Department, int Page = 1, int PageSize = 20)
    : MediatR.IRequest<IReadOnlyList<PurchaseRequestDto>>;
}
