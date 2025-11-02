using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "USD";

        private Money() { } // for EF
        public Money(decimal amount, string currency)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            Amount = amount;
        }

        public static Money Zero(string currency = "USD") => new(0m, currency);

        public Money Add(Money other)
        {
            if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Cannot add money with different currencies.");
            return new Money(Amount + other.Amount, Currency);
        }
    }
}
