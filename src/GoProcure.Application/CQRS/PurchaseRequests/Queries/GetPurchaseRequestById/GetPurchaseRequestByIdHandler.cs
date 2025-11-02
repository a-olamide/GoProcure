using GoProcure.Application.Abstractions.ReadModels;
using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Queries.GetPurchaseRequestById
{
    public sealed class GetPurchaseRequestByIdHandler
    : MediatR.IRequestHandler<GetPurchaseRequestByIdQuery, PurchaseRequestDto?>
    {
        private readonly IPurchaseRequestReadModel _read;
        public GetPurchaseRequestByIdHandler(IPurchaseRequestReadModel read) => _read = read;
        public Task<PurchaseRequestDto?> Handle(GetPurchaseRequestByIdQuery q, CancellationToken ct)
            => _read.GetByIdAsync(q.Id, ct);
    }
}
