using GoProcure.Domain.Common;
using GoProcure.Domain.Enums;
using GoProcure.Domain.Events;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class PurchaseRequest : AuditableEntity<Guid>
    {
        private readonly List<PurchaseRequestLine> _lines = new();
        private readonly List<Approval> _approvals = new();
        private List<Guid> _approvalRoute = new();

        public IReadOnlyCollection<PurchaseRequestLine> Lines => _lines.AsReadOnly();
        public IReadOnlyCollection<Approval> Approvals => _approvals.AsReadOnly();
        public IReadOnlyList<Guid> ApprovalRoute => _approvalRoute.AsReadOnly();

        public Guid RequesterId { get; private set; }
        public string Department { get; private set; } = default!;
        public PRStatus Status { get; private set; } = PRStatus.Draft;
        public Money Total { get; private set; } = Money.Zero();
        public Money? BudgetLimit { get; private set; }

        private PurchaseRequest() { }

        public PurchaseRequest(Guid requesterId, string department, Money? budgetLimit = null)
        {
            Id = Guid.NewGuid();
            if (requesterId == Guid.Empty) throw new DomainException("RequesterId required.");
            if (string.IsNullOrWhiteSpace(department)) throw new DomainException("Department required.");
            RequesterId = requesterId; 
            Department = department.Trim(); 
            BudgetLimit = budgetLimit;

            AddDomainEvent(new PurchaseRequestCreated(Id, requesterId));
        }

        public void SetApprovalRoute(IEnumerable<Guid> approverIds)
        {
            var route = approverIds?.Where(x => x != Guid.Empty).Distinct().ToList() ?? new();
            if (route.Count == 0) throw new DomainException("Approval route cannot be empty.");
            _approvalRoute = route;
        }

        public void AddLine(Guid itemId, string description, int qty, Money unitPrice, Guid? suggestedVendorId = null)
        {
            EnsureEditable();
            if (qty <= 0) throw new DomainException("Quantity must be positive.");
            if (unitPrice.Amount <= 0) throw new DomainException("Unit price must be positive.");
            _lines.Add(new PurchaseRequestLine(Id, itemId, description, qty, unitPrice, suggestedVendorId));
            RecomputeTotalAndCheckBudget();
        }

        public void UpdateLine(Guid lineId, int qty, Money unitPrice, Guid? suggestedVendorId = null)
        {
            EnsureEditable();
            var line = _lines.FirstOrDefault(l => l.Id == lineId) ?? throw new DomainException("Line not found.");
            if (qty <= 0) throw new DomainException("Quantity must be positive.");
            if (unitPrice.Amount <= 0) throw new DomainException("Unit price must be positive.");
            line.Update(qty, unitPrice, suggestedVendorId);
            RecomputeTotalAndCheckBudget();
        }

        public void RemoveLine(Guid lineId)
        {
            EnsureEditable();
            var removed = _lines.RemoveAll(l => l.Id == lineId);
            if (removed == 0) throw new DomainException("Line not found.");
            if (_lines.Count == 0) throw new DomainException("PR must have at least one line.");
            RecomputeTotalAndCheckBudget();
        }

        public void Submit()
        {
            if (_lines.Count == 0) throw new DomainException("PR must have at least one line.");
            if (_approvalRoute.Count == 0) throw new DomainException("Approval route not assigned.");
            Status = PRStatus.PendingApproval;
            AddDomainEvent(new PurchaseRequestSubmitted(Id, Total));
        }

        public void ApplyDecision(ApprovalDecision decision, Guid approverId, string? comment)
        {
            if (Status != PRStatus.PendingApproval) throw new DomainException("Not pending approval.");
            _approvals.Add(new Approval(Id, approverId, decision, comment));
            Status = decision switch
            {
                ApprovalDecision.Approve => PRStatus.Approved,
                ApprovalDecision.Reject => PRStatus.Rejected,
                ApprovalDecision.RequestChanges => PRStatus.Draft,
                _ => Status
            };
            AddDomainEvent(new PurchaseRequestDecided(Id, approverId, decision));
        }

        private void EnsureEditable()
        {
            if (Status != PRStatus.Draft) throw new DomainException("Only Draft PR is editable.");
        }

        private void RecomputeTotalAndCheckBudget()
        {
            Total = _lines.Aggregate(Money.Zero(), (acc, l) => acc.Add(l.Subtotal));
            if (BudgetLimit is not null && Total.Amount > BudgetLimit.Amount)
                throw new DomainException($"Total {Total.Amount} exceeds budget {BudgetLimit.Amount}.");
        }
    }
}
