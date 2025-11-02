using GoProcure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoProcure.Domain.ValueObjects
{
    public sealed record PhoneNumber
    {
        public string Value { get; init; } = default!;

        // Accept + and digits, 8..15 digits total (common E.164 range)
        private static readonly Regex E164 = new(@"^\+[1-9]\d{7,14}$", RegexOptions.Compiled);

        private PhoneNumber() //for EF
        {
            
        }
        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(value) || !E164.IsMatch(value))
                throw new ArgumentException("Invalid email.");
            Value = value;
        }
        public static PhoneNumber Create(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new DomainException("Phone number is required.");

            // strip formatting: spaces, dashes, parentheses, dots
            var normalized = Normalize(raw);

            if (!E164.IsMatch(normalized))
                throw new DomainException("Invalid phone number format. Use international format like +15551234567.");

            return new PhoneNumber(normalized);
        }

        public static bool TryCreate(string raw, out PhoneNumber phone)
        {
            try { phone = Create(raw); return true; }
            catch { phone = default; return false; }
        }

        public override string ToString() => Value;

        private static string Normalize(string input)
        {
            // keep + and digits; drop other characters
            var trimmed = input.Trim();
            var chars = new List<char>(trimmed.Length);
            foreach (var c in trimmed)
            {
                if (char.IsDigit(c) || c == '+') chars.Add(c);
            }

            // ensure + only as first char
            var result = new string(chars.ToArray());
            if (result.Length > 0 && result[0] != '+')
            {
                // If user typed local like 1555..., we can't infer country code safely
                // Force explicit +<country><number>
                result = "+" + result; // optional: or throw to force user to supply '+'
            }

            // collapse any accidental +'s; keep first only
            var plusIndex = result.IndexOf('+');
            if (plusIndex > 0) result = result.Replace("+", ""); // remove all then add one
            if (!result.StartsWith("+")) result = "+" + result;

            return result;
        }
    }
}
