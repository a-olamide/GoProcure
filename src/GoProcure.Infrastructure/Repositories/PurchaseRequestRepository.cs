using GoProcure.Application.Abstractions.Repositories;
using GoProcure.Domain.Entities;
using GoProcure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Repositories
{
    public sealed class PurchaseRequestRepository : IPurchaseRequestRepository
    {
        private readonly AppDbContext _db;
        public PurchaseRequestRepository(AppDbContext db) => _db = db;
        public Task<PurchaseRequest?> GetByIdAsync(Guid id, CancellationToken ct) =>
         _db.PurchaseRequests
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<PurchaseRequest?> GetForSubmissionAsync(Guid id, CancellationToken ct) =>
            _db.PurchaseRequests
               .Include(p => p.Approvals)
               .Include(p => p.Lines)            // if you exposed a nav; otherwise remove
               .FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task AddAsync(PurchaseRequest pr, CancellationToken ct) =>
            _db.PurchaseRequests.AddAsync(pr, ct).AsTask();
    }
}
