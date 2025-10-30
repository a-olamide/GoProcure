using GoProcure.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.ApplyApprovalDecision
{
    public sealed record ApplyApprovalDecisionCommand(
    Guid PurchaseRequestId,
    Guid ApproverId,
    ApprovalDecision Decision,
    string? Comment
) : IRequest<Unit>;
}
