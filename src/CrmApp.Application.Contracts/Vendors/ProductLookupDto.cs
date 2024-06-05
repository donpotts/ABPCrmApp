using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Vendors;

public class ProductLookupDto : EntityDto<int>
{
	public string? Name { get; set; }
}
