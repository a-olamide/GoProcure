using GoProcure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoProcure.Domain.ValueObjects
{
    public readonly record struct Email(string Value)
    {
        private static readonly Regex Rx = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Rx.IsMatch(value))
                throw new DomainException("Invalid email.");
            return new Email(value.Trim().ToLowerInvariant());
        }

        public override string ToString() => Value;
    }
}
