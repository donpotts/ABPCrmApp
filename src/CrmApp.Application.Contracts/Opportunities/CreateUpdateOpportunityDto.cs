using System;

namespace CrmApp.Opportunities;

public class CreateUpdateOpportunityDto
{
    public DateTime EstimatedCloseDate { get; set; }

    public string? Stage { get; set; }

    public string? Icon { get; set; }

    public string? Notes { get; set; }
}
