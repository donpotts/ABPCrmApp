using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CrmApp.Contacts;

public class Contact : AuditedAggregateRoot<int>
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public int Phone { get; set; }

    public string? Role { get; set; }

    public int AddressId { get; set; }

    public string? Photo { get; set; }

    public int RewardId { get; set; }

    public string? Notes { get; set; }
}
