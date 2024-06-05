using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Addresses;

public class AddressDto : AuditedEntityDto<int>
{
    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int ZipCode { get; set; }

    public string? Country { get; set; }

    public string? Photo { get; set; }

    public string? Notes { get; set; }
}
