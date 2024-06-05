using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Vendors;

public class VendorDto : AuditedEntityDto<int>
{
    public string? Name { get; set; }

    public string? ContactName { get; set; }

    public int Phone { get; set; }

    public string? Email { get; set; }

    public int AddressId { get; set; }

    public int ProductId { get; set; }

    public int ServiceId { get; set; }

    public string? Logo { get; set; }

    public string? Notes { get; set; }

    public string? AddressCity { get; set; }

    public string? ServiceName { get; set; }

    public string? ProductName { get; set; }
}
