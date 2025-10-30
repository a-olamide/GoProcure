using GoProcure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstraction.Policies
{
    public interface IApprovalRoutingService
    {
        IReadOnlyList<Guid> BuildRoute(PurchaseRequest pr);
    }
}
