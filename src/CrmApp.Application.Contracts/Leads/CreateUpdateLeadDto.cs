using System;

namespace CrmApp.Leads;

public class CreateUpdateLeadDto
{
    public int ContactId { get; set; }

    public string? Source { get; set; }

    public string? Status { get; set; }

    public double PotentialValue { get; set; }

    public int OpportunityId { get; set; }

    public int AddressId { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }
}
