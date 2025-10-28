using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class Payment : AuditableEntity<Guid>
    {
        public Guid InvoiceId { get; private set; }
        public Money Amount { get; private set; }
        public string Method { get; private set; } = "BankTransfer";
        public string Reference { get; private set; } = default!;
        public DateTime PaidAtUtc { get; private set; } = DateTime.UtcNow;

        private Payment() { }

        public Payment(Guid invoiceId, Money amount, string method, string reference, DateTime paidAtUtc)
        {
            Id = Guid.NewGuid();
            if (invoiceId == Guid.Empty) throw new DomainException("InvoiceId required.");
            if (amount.Amount <= 0) throw new DomainException("Payment amount must be positive.");
            if (string.IsNullOrWhiteSpace(reference)) throw new DomainException("Reference required.");
            InvoiceId = invoiceId; Amount = amount; Method = method?.Trim() ?? "BankTransfer"; Reference = reference.Trim(); PaidAtUtc = paidAtUtc;
        }
    }
}
