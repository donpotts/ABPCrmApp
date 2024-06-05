using System;
using CrmApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Addresses;

public class AddressAppService :
    CrudAppService<
        Address, //The Address entity
        AddressDto, //Used to show addresses
        int, //Primary key of the address entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateAddressDto>, //Used to create/update a address
    IAddressAppService //implement the IAddressAppService
{
    public AddressAppService(IRepository<Address, int> repository)
        : base(repository)
    {
        GetPolicyName = CrmAppPermissions.Addresses.Default;
        GetListPolicyName = CrmAppPermissions.Addresses.Default;
        CreatePolicyName = CrmAppPermissions.Addresses.Create;
        UpdatePolicyName = CrmAppPermissions.Addresses.Edit;
        DeletePolicyName = CrmAppPermissions.Addresses.Delete;
    }
}
