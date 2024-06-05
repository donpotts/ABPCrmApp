using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CrmApp.Addresses;
using CrmApp.Opportunities;
using CrmApp.Contacts;
using CrmApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Leads;

[Authorize(CrmAppPermissions.Leads.Default)]
public class LeadAppService :
    CrudAppService<
        Lead, //The Lead entity
        LeadDto, //Used to show leads
        int, //Primary key of the lead entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateLeadDto>, //Used to create/update a lead
    ILeadAppService //implement the ILeadAppService
{
    private readonly IRepository<Address, int> _addressRepository;
    private readonly IRepository<Opportunity, int> _opportunityRepository;
    private readonly IRepository<Contact, int> _contactRepository;

    public LeadAppService(
        IRepository<Lead, int> repository,
        IRepository<Address, int> addressRepository,
        IRepository<Opportunity, int> opportunityRepository,
        IRepository<Contact, int> contactRepository)
        : base(repository)
    {
        _addressRepository = addressRepository;
        _opportunityRepository = opportunityRepository;
        _contactRepository = contactRepository;
        GetPolicyName = CrmAppPermissions.Leads.Default;
        GetListPolicyName = CrmAppPermissions.Leads.Default;
        CreatePolicyName = CrmAppPermissions.Leads.Create;
        UpdatePolicyName = CrmAppPermissions.Leads.Edit;
        DeletePolicyName = CrmAppPermissions.Leads.Delete;
    }

    public override async Task<LeadDto> GetAsync(int id)
    {
        //Get the IQueryable<Lead> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join leads and addresses, opportunities, contacts
        var query = from lead in queryable
            join address in await _addressRepository.GetQueryableAsync() on lead.AddressId equals address.Id
            join opportunity in await _opportunityRepository.GetQueryableAsync() on lead.OpportunityId equals opportunity.Id
            join contact in await _contactRepository.GetQueryableAsync() on lead.ContactId equals contact.Id
            where lead.Id == id
            select new { lead, address, opportunity, contact };

        //Execute the query and get the lead with address, opportunity, contact
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Lead), id);
        }

        var leadDto = ObjectMapper.Map<Lead, LeadDto>(queryResult.lead);
        leadDto.AddressCity = queryResult.address.City;
        leadDto.OpportunityStage = queryResult.opportunity.Stage;
        leadDto.ContactName = queryResult.contact.Name;
        return leadDto;
    }

    public override async Task<PagedResultDto<LeadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Lead> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join leads and addresses, opportunities, contacts
        var query = from lead in queryable
            join address in await _addressRepository.GetQueryableAsync() on lead.AddressId equals address.Id
            join opportunity in await _opportunityRepository.GetQueryableAsync() on lead.OpportunityId equals opportunity.Id
            join contact in await _contactRepository.GetQueryableAsync() on lead.ContactId equals contact.Id
            select new {lead, address, opportunity, contact};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of LeadDto objects
        var leadDtos = queryResult.Select(x =>
        {
            var leadDto = ObjectMapper.Map<Lead, LeadDto>(x.lead);
            leadDto.AddressCity = x.address.City;
            leadDto.OpportunityStage = x.opportunity.Stage;
            leadDto.ContactName = x.contact.Name;
            return leadDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<LeadDto>(
            totalCount,
            leadDtos
        );
    }

    public async Task<ListResultDto<AddressLookupDto>> GetAddressLookupAsync()
    {
        var addresses = await _addressRepository.GetListAsync();

        return new ListResultDto<AddressLookupDto>(
            ObjectMapper.Map<List<Address>, List<AddressLookupDto>>(addresses)
        );
    }

    public async Task<ListResultDto<OpportunityLookupDto>> GetOpportunityLookupAsync()
    {
        var opportunities = await _opportunityRepository.GetListAsync();

        return new ListResultDto<OpportunityLookupDto>(
            ObjectMapper.Map<List<Opportunity>, List<OpportunityLookupDto>>(opportunities)
        );
    }

    public async Task<ListResultDto<ContactLookupDto>> GetContactLookupAsync()
    {
        var contacts = await _contactRepository.GetListAsync();

        return new ListResultDto<ContactLookupDto>(
            ObjectMapper.Map<List<Contact>, List<ContactLookupDto>>(contacts)
        );
    }

    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"lead.{nameof(Lead.Id)}";
        }

        if (sorting.Contains("addressCity", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "addressCity",
                "Address.City",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("opportunityStage", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "opportunityStage",
                "Opportunity.Stage",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("contactName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "contactName",
                "Contact.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"lead.{sorting}";
    }
}
