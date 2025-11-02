using GoProcure.Application.Abstraction.Policies;
using GoProcure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Policies
{
    public sealed class ApprovalRoutingService : IApprovalRoutingService
    {
        // Replace with threshold/role logic later (e.g., read from DB/config).
        public IReadOnlyList<Guid> BuildRoute(PurchaseRequest pr)
        {
            // Demo: one approver if total <= 10k, two if > 10k
            var approver1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var approver2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

            return pr.Total.Amount <= 10_000m
                ? new[] { approver1 }
                : new[] { approver1, approver2 };
        }
    }
}
