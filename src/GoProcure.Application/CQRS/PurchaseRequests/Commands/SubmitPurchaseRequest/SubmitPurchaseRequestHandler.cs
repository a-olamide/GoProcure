using GoProcure.Application.Abstraction;
using GoProcure.Application.Abstraction.Policies;
using GoProcure.Application.Abstractions.Persistence;
using GoProcure.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.SubmitPurchaseRequest
{
    public sealed class SubmitPurchaseRequestHandler
    : IRequestHandler<SubmitPurchaseRequestCommand, Unit>
    {
        private readonly IPurchaseRequestRepository _repo;
        private readonly IApprovalRoutingService _routing;
        private readonly IUnitOfWork _uow;

        public SubmitPurchaseRequestHandler(
            IPurchaseRequestRepository repo,
            IApprovalRoutingService routing,
            IUnitOfWork uow)
        { _repo = repo; _routing = routing; _uow = uow; }

        public async Task<Unit> Handle(SubmitPurchaseRequestCommand request, CancellationToken ct)
        {
            var pr = await _repo.GetForSubmissionAsync(request.PurchaseRequestId, ct)
                     ?? throw new KeyNotFoundException("PurchaseRequest not found.");

            var route = _routing.BuildRoute(pr);
            pr.SetApprovalRoute(route);
            pr.Submit();

            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
