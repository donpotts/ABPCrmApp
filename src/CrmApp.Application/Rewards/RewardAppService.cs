using System;
using CrmApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Rewards;

public class RewardAppService :
    CrudAppService<
        Reward, //The Reward entity
        RewardDto, //Used to show rewards
        int, //Primary key of the reward entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateRewardDto>, //Used to create/update a reward
    IRewardAppService //implement the IRewardAppService
{
    public RewardAppService(IRepository<Reward, int> repository)
        : base(repository)
    {
        GetPolicyName = CrmAppPermissions.Rewards.Default;
        GetListPolicyName = CrmAppPermissions.Rewards.Default;
        CreatePolicyName = CrmAppPermissions.Rewards.Create;
        UpdatePolicyName = CrmAppPermissions.Rewards.Edit;
        DeletePolicyName = CrmAppPermissions.Rewards.Delete;
    }
}
