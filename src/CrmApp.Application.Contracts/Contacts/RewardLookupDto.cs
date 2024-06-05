using System;
using Volo.Abp.Application.Dtos;

namespace CrmApp.Contacts;

public class RewardLookupDto : EntityDto<int>
{
	public int Rewardpoints { get; set; }
}
