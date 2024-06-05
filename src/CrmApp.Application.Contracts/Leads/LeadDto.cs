using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Leads;

public class LeadDto : AuditedEntityDto<int>
{
    public int ContactId { get; set; }

    public string? Source { get; set; }

    public string? Status { get; set; }

    public double PotentialValue { get; set; }

    public int OpportunityId { get; set; }

    public int AddressId { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }

    public string? AddressCity { get; set; }

    public string? OpportunityStage { get; set; }

    public string? ContactName { get; set; }
}
