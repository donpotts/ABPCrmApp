using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CrmApp.Vendors;

public class Vendor : AuditedAggregateRoot<int>
{
    public string? Name { get; set; }

    public string? ContactName { get; set; }

    public int Phone { get; set; }

    public string? Email { get; set; }

    public int AddressId { get; set; }

    public int ProductId { get; set; }

    public int ServiceId { get; set; }

    public string? Logo { get; set; }

    public string? Notes { get; set; }
}
