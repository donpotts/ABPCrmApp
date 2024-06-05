using System;

namespace CrmApp.Products;

public class CreateUpdateProductDto
{
    public int ProductCategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }
}
