using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Products;

public class ProductDto : AuditedEntityDto<int>
{
    public int ProductCategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }

    public string? ProductCategoryName { get; set; }
}
