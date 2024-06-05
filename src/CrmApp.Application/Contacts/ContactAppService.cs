using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CrmApp.Addresses;
using CrmApp.Rewards;
using CrmApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Contacts;

[Authorize(CrmAppPermissions.Contacts.Default)]
public class ContactAppService :
    CrudAppService<
        Contact, //The Contact entity
        ContactDto, //Used to show contacts
        int, //Primary key of the contact entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateContactDto>, //Used to create/update a contact
    IContactAppService //implement the IContactAppService
{
    private readonly IRepository<Address, int> _addressRepository;
    private readonly IRepository<Reward, int> _rewardRepository;

    public ContactAppService(
        IRepository<Contact, int> repository,
        IRepository<Address, int> addressRepository,
        IRepository<Reward, int> rewardRepository)
        : base(repository)
    {
        _addressRepository = addressRepository;
        _rewardRepository = rewardRepository;
        GetPolicyName = CrmAppPermissions.Contacts.Default;
        GetListPolicyName = CrmAppPermissions.Contacts.Default;
        CreatePolicyName = CrmAppPermissions.Contacts.Create;
        UpdatePolicyName = CrmAppPermissions.Contacts.Edit;
        DeletePolicyName = CrmAppPermissions.Contacts.Delete;
    }

    public override async Task<ContactDto> GetAsync(int id)
    {
        //Get the IQueryable<Contact> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join contacts and addresses, rewards
        var query = from contact in queryable
            join address in await _addressRepository.GetQueryableAsync() on contact.AddressId equals address.Id
            join reward in await _rewardRepository.GetQueryableAsync() on contact.RewardId equals reward.Id
            where contact.Id == id
            select new { contact, address, reward };

        //Execute the query and get the contact with address, reward
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Contact), id);
        }

        var contactDto = ObjectMapper.Map<Contact, ContactDto>(queryResult.contact);
        contactDto.AddressCity = queryResult.address.City;
        contactDto.RewardRewardpoints = queryResult.reward.Rewardpoints;
        return contactDto;
    }

    public override async Task<PagedResultDto<ContactDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Contact> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join contacts and addresses, rewards
        var query = from contact in queryable
            join address in await _addressRepository.GetQueryableAsync() on contact.AddressId equals address.Id
            join reward in await _rewardRepository.GetQueryableAsync() on contact.RewardId equals reward.Id
            select new {contact, address, reward};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of ContactDto objects
        var contactDtos = queryResult.Select(x =>
        {
            var contactDto = ObjectMapper.Map<Contact, ContactDto>(x.contact);
            contactDto.AddressCity = x.address.City;
            contactDto.RewardRewardpoints = x.reward.Rewardpoints;
            return contactDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<ContactDto>(
            totalCount,
            contactDtos
        );
    }

    public async Task<ListResultDto<AddressLookupDto>> GetAddressLookupAsync()
    {
        var addresses = await _addressRepository.GetListAsync();

        return new ListResultDto<AddressLookupDto>(
            ObjectMapper.Map<List<Address>, List<AddressLookupDto>>(addresses)
        );
    }

    public async Task<ListResultDto<RewardLookupDto>> GetRewardLookupAsync()
    {
        var rewards = await _rewardRepository.GetListAsync();

        return new ListResultDto<RewardLookupDto>(
            ObjectMapper.Map<List<Reward>, List<RewardLookupDto>>(rewards)
        );
    }

    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"contact.{nameof(Contact.Id)}";
        }

        if (sorting.Contains("addressCity", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "addressCity",
                "Address.City",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("rewardRewardpoints", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "rewardRewardpoints",
                "Reward.Rewardpoints",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"contact.{sorting}";
    }
}
