using System;
using CrmApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.ProductCategories;

public class ProductCategoryAppService :
    CrudAppService<
        ProductCategory, //The ProductCategory entity
        ProductCategoryDto, //Used to show product categories
        int, //Primary key of the product category entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProductCategoryDto>, //Used to create/update a product category
    IProductCategoryAppService //implement the IProductCategoryAppService
{
    public ProductCategoryAppService(IRepository<ProductCategory, int> repository)
        : base(repository)
    {
        GetPolicyName = CrmAppPermissions.ProductCategories.Default;
        GetListPolicyName = CrmAppPermissions.ProductCategories.Default;
        CreatePolicyName = CrmAppPermissions.ProductCategories.Create;
        UpdatePolicyName = CrmAppPermissions.ProductCategories.Edit;
        DeletePolicyName = CrmAppPermissions.ProductCategories.Delete;
    }
}
