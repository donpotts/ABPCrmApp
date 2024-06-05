using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace CrmApp.Leads;

public interface ILeadAppService :
    ICrudAppService< //Defines CRUD methods
        LeadDto, //Used to show leads
        int, //Primary key of the lead entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateLeadDto> //Used to create/update a lead
{
    Task<ListResultDto<AddressLookupDto>> GetAddressLookupAsync();

    Task<ListResultDto<OpportunityLookupDto>> GetOpportunityLookupAsync();

    Task<ListResultDto<ContactLookupDto>> GetContactLookupAsync();
}
