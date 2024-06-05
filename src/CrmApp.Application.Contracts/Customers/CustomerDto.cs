using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Customers;

public class CustomerDto : AuditedEntityDto<int>
{
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Industry { get; set; }

    public int AddressId { get; set; }

    public int ContactId { get; set; }

    public string? Logo { get; set; }

    public string? Notes { get; set; }

    public string? AddressCity { get; set; }

    public string? ContactName { get; set; }
}
