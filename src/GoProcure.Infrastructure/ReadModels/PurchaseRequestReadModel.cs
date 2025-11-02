using GoProcure.Application.Abstractions.ReadModels;
using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using GoProcure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.ReadModels
{
    public sealed class PurchaseRequestReadModel : IPurchaseRequestReadModel
    {
        private readonly AppDbContext _db;
        public PurchaseRequestReadModel(AppDbContext db) => _db = db;

        public async Task<PurchaseRequestDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.PurchaseRequests
                .AsNoTracking()
                .Select(p => new PurchaseRequestDto
                {
                    Id = p.Id,
                    RequesterId = p.RequesterId,
                    Department = p.Department,
                    Status = p.Status,
                    Total = p.Total.Amount,
                    Currency = p.Total.Currency
                })
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<IReadOnlyList<PurchaseRequestDto>> SearchAsync(string? department, int page, int pageSize, CancellationToken ct)
        {
            var query = _db.PurchaseRequests.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(department))
                query = query.Where(x => x.Department == department);

            return await query
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(p => new PurchaseRequestDto
                {
                    Id = p.Id,
                    RequesterId = p.RequesterId,
                    Department = p.Department,
                    Status = p.Status,
                    Total = p.Total.Amount,
                    Currency = p.Total.Currency
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }
    }
}
