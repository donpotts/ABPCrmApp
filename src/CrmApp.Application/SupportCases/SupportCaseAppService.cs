using System;
using CrmApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.SupportCases;

public class SupportCaseAppService :
    CrudAppService<
        SupportCase, //The SupportCase entity
        SupportCaseDto, //Used to show support cases
        int, //Primary key of the support case entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateSupportCaseDto>, //Used to create/update a support case
    ISupportCaseAppService //implement the ISupportCaseAppService
{
    public SupportCaseAppService(IRepository<SupportCase, int> repository)
        : base(repository)
    {
        GetPolicyName = CrmAppPermissions.SupportCases.Default;
        GetListPolicyName = CrmAppPermissions.SupportCases.Default;
        CreatePolicyName = CrmAppPermissions.SupportCases.Create;
        UpdatePolicyName = CrmAppPermissions.SupportCases.Edit;
        DeletePolicyName = CrmAppPermissions.SupportCases.Delete;
    }
}
