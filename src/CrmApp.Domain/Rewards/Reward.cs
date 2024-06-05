using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CrmApp.Rewards;

public class Reward : AuditedAggregateRoot<int>
{
    public int Rewardpoints { get; set; }

    public double CreditsDollars { get; set; }

    public string? ConversionRate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string? Notes { get; set; }
}
