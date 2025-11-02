using GoProcure.Application.Abstractions.ReadModels;
using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Queries.SearchPurchaseRequests
{
    public sealed class SearchPurchaseRequestsHandler
    : MediatR.IRequestHandler<SearchPurchaseRequestsQuery, IReadOnlyList<PurchaseRequestDto>>
    {
        private readonly IPurchaseRequestReadModel _read;
        public SearchPurchaseRequestsHandler(IPurchaseRequestReadModel read) => _read = read;
        public Task<IReadOnlyList<PurchaseRequestDto>> Handle(SearchPurchaseRequestsQuery q, CancellationToken ct)
            => _read.SearchAsync(q.Department, q.Page, q.PageSize, ct);
    }
}
