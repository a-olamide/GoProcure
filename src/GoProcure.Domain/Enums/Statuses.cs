using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Enums
{
    public enum PRStatus { Draft, PendingApproval, Approved, Rejected, Canceled }
    public enum POStatus { Draft, Sent, PartiallyReceived, FullyReceived, Canceled }
    public enum InvoiceStatus { Submitted, Matched, Mismatch, ExceptionApproved, Rejected, Paid }
    public enum ApprovalDecision { Pending, Approve, Reject, RequestChanges }
}
