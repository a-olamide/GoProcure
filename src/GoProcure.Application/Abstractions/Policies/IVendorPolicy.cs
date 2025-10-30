using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstraction.Policies
{
    public interface IVendorPolicy
    {
        Task<bool> IsApprovedAsync(Guid vendorId, CancellationToken ct);
        Task<bool> SuppliesItemAsync(Guid vendorId, Guid itemId, CancellationToken ct);
    }
}
