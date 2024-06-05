using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Customers;

public class AddressLookupDto : EntityDto<int>
{
	public string? City { get; set; }
}
