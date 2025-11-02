using GoProcure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoProcure.Domain.ValueObjects
{
    public sealed record Email
    {
        public string Value { get; init; } = default!;
        private static readonly Regex Rx = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private Email()
        {
            
        }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(value) || !Rx.IsMatch(value))
                throw new ArgumentException("Invalid email.");
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            value = value.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(value) || !Rx.IsMatch(value))
                throw new DomainException("Invalid email.");
            return new Email(value.Trim().ToLowerInvariant());
        }

        public override string ToString() => Value;
    }
}
