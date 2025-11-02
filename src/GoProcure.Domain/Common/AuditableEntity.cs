using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Common
{
    public abstract class AuditableEntity<TId> : Entity<TId>
    {
        public DateTime CreatedAtUtc { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? ModifiedAtUtc { get; protected set; }
        public string? ModifiedBy { get; protected set; }

        public bool IsDeleted { get; protected set; }

        [Timestamp] public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();

        public void SetCreatedAtUtc()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void SetModifiedAtUtc()
        {
            ModifiedAtUtc = DateTime.UtcNow;
        }

        public void SoftDelete() => IsDeleted = true;
    }
}
