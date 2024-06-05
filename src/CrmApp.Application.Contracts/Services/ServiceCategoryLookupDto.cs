using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Services;

public class ServiceCategoryLookupDto : EntityDto<int>
{
	public string? Name { get; set; }
}
