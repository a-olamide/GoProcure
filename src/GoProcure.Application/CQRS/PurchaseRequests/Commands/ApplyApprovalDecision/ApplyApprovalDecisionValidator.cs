using FluentValidation;
using GoProcure.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.ApplyApprovalDecision
{
    public sealed class ApplyApprovalDecisionValidator : AbstractValidator<ApplyApprovalDecisionCommand>
    {
        public ApplyApprovalDecisionValidator()
        {
            RuleFor(x => x.PurchaseRequestId).NotEmpty();
            RuleFor(x => x.ApproverId).NotEmpty();
            RuleFor(x => x.Decision).IsInEnum().Must(d => d != ApprovalDecision.Pending);
            RuleFor(x => x.Comment).MaximumLength(500);
        }
    }
}
