using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Services;

public class ServiceDto : AuditedEntityDto<int>
{
    public int ServiceCategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Recurring { get; set; }

    public string? Icon { get; set; }

    public string? Notes { get; set; }

    public string? ServiceCategoryName { get; set; }
}
