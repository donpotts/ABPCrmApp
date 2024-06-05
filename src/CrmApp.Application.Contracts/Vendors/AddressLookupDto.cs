using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Vendors;

public class AddressLookupDto : EntityDto<int>
{
	public string? City { get; set; }
}
