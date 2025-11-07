using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.IAM.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        // Basic identity fields
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? Department { get; set; }

        // Optional profile fields
        public string? JobTitle { get; set; }
        public string? Organization { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;

        // Computed convenience property (not mapped)
        public string FullName => $"{FirstName} {LastName}".Trim();

    }
}
