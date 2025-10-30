using GoProcure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstractions.Repositories
{
    public interface IPurchaseRequestRepository
    {
        Task<PurchaseRequest?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<PurchaseRequest?> GetForSubmissionAsync(Guid id, CancellationToken ct); // includes approvals/lines as needed
        Task AddAsync(PurchaseRequest pr, CancellationToken ct);
    }
}
