using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.ValueObjects
{
    public readonly record struct Money(decimal Amount, string Currency)
    {
        public static Money Zero(string currency = "USD") => new(0m, currency);

        public Money Add(Money other)
            => Currency == other.Currency
               ? new Money(Amount + other.Amount, Currency)
               : throw new InvalidOperationException("Currency mismatch.");
    }
}
