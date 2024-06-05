using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace CrmApp.Products;

public class Product : AuditedAggregateRoot<int>
{
    public int ProductCategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }
}
