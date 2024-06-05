using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Vendors;

public class ServiceLookupDto : EntityDto<int>
{
	public string? Name { get; set; }
}
