using GoProcure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstractions.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
