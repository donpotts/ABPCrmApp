using System;
using CrmApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Sales;

public class SaleAppService :
    CrudAppService<
        Sale, //The Sale entity
        SaleDto, //Used to show sales
        int, //Primary key of the sale entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateSaleDto>, //Used to create/update a sale
    ISaleAppService //implement the ISaleAppService
{
    public SaleAppService(IRepository<Sale, int> repository)
        : base(repository)
    {
        GetPolicyName = CrmAppPermissions.Sales.Default;
        GetListPolicyName = CrmAppPermissions.Sales.Default;
        CreatePolicyName = CrmAppPermissions.Sales.Create;
        UpdatePolicyName = CrmAppPermissions.Sales.Edit;
        DeletePolicyName = CrmAppPermissions.Sales.Delete;
    }
}
