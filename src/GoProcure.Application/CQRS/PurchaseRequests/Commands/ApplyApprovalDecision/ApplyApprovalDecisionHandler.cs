using GoProcure.Application.Abstractions.Persistence;
using GoProcure.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.ApplyApprovalDecision
{

    public sealed class ApplyApprovalDecisionHandler
        : IRequestHandler<ApplyApprovalDecisionCommand, Unit>
    {
        private readonly IPurchaseRequestRepository _repo;
        private readonly IUnitOfWork _uow;

        public ApplyApprovalDecisionHandler(IPurchaseRequestRepository repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<Unit> Handle(ApplyApprovalDecisionCommand request, CancellationToken ct)
        {
            var pr = await _repo.GetForSubmissionAsync(request.PurchaseRequestId, ct)
                     ?? throw new KeyNotFoundException("PurchaseRequest not found.");

            pr.ApplyDecision(request.Decision, request.ApproverId, request.Comment);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
