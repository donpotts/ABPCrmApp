using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CrmApp.Customers;

public class Customer : AuditedAggregateRoot<int>
{
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Industry { get; set; }

    public int AddressId { get; set; }

    public int ContactId { get; set; }

    public string? Logo { get; set; }

    public string? Notes { get; set; }
}
