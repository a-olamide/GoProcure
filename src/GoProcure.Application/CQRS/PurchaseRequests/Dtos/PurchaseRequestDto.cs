using GoProcure.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Dtos
{
    public sealed class PurchaseRequestDto
    {
        public Guid Id { get; init; }
        public Guid RequesterId { get; init; }
        public string Department { get; init; } = default!;
        public PRStatus Status { get; init; }
        public decimal Total { get; init; }
        public string Currency { get; init; } = "USD";
    }
}
