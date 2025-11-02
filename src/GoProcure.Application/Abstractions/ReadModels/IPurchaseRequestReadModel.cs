using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstractions.ReadModels
{
    public interface IPurchaseRequestReadModel
    {
        Task<PurchaseRequestDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<PurchaseRequestDto>> SearchAsync(string? department, int page, int pageSize, CancellationToken ct);
    }
}
