using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CrmApp.Services;

public class Service : AuditedAggregateRoot<int>
{
    public int ServiceCategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Recurring { get; set; }

    public string? Icon { get; set; }

    public string? Notes { get; set; }
}
