using GoProcure.Domain.Common;
using GoProcure.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class Approval : AuditableEntity<Guid>
    {
        public Guid PurchaseRequestId { get; private set; }
        public Guid ApproverId { get; private set; }
        public ApprovalDecision Decision { get; private set; }
        public string? Comment { get; private set; }

        private Approval() { }

        public Approval(Guid prId, Guid approverId, ApprovalDecision decision, string? comment)
        {
            Id = Guid.NewGuid();
            PurchaseRequestId = prId;
            ApproverId = approverId;
            Decision = decision;
            Comment = comment?.Trim();
        }
    }
}
