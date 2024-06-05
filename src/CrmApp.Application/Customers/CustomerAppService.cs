using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CrmApp.Addresses;
using CrmApp.Contacts;
using CrmApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Customers;

[Authorize(CrmAppPermissions.Customers.Default)]
public class CustomerAppService :
    CrudAppService<
        Customer, //The Customer entity
        CustomerDto, //Used to show customers
        int, //Primary key of the customer entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateCustomerDto>, //Used to create/update a customer
    ICustomerAppService //implement the ICustomerAppService
{
    private readonly IRepository<Address, int> _addressRepository;
    private readonly IRepository<Contact, int> _contactRepository;

    public CustomerAppService(
        IRepository<Customer, int> repository,
        IRepository<Address, int> addressRepository,
        IRepository<Contact, int> contactRepository)
        : base(repository)
    {
        _addressRepository = addressRepository;
        _contactRepository = contactRepository;
        GetPolicyName = CrmAppPermissions.Customers.Default;
        GetListPolicyName = CrmAppPermissions.Customers.Default;
        CreatePolicyName = CrmAppPermissions.Customers.Create;
        UpdatePolicyName = CrmAppPermissions.Customers.Edit;
        DeletePolicyName = CrmAppPermissions.Customers.Delete;
    }

    public override async Task<CustomerDto> GetAsync(int id)
    {
        //Get the IQueryable<Customer> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join customers and addresses, contacts
        var query = from customer in queryable
            join address in await _addressRepository.GetQueryableAsync() on customer.AddressId equals address.Id
            join contact in await _contactRepository.GetQueryableAsync() on customer.ContactId equals contact.Id
            where customer.Id == id
            select new { customer, address, contact };

        //Execute the query and get the customer with address, contact
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Customer), id);
        }

        var customerDto = ObjectMapper.Map<Customer, CustomerDto>(queryResult.customer);
        customerDto.AddressCity = queryResult.address.City;
        customerDto.ContactName = queryResult.contact.Name;
        return customerDto;
    }

    public override async Task<PagedResultDto<CustomerDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Customer> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join customers and addresses, contacts
        var query = from customer in queryable
            join address in await _addressRepository.GetQueryableAsync() on customer.AddressId equals address.Id
            join contact in await _contactRepository.GetQueryableAsync() on customer.ContactId equals contact.Id
            select new {customer, address, contact};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of CustomerDto objects
        var customerDtos = queryResult.Select(x =>
        {
            var customerDto = ObjectMapper.Map<Customer, CustomerDto>(x.customer);
            customerDto.AddressCity = x.address.City;
            customerDto.ContactName = x.contact.Name;
            return customerDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<CustomerDto>(
            totalCount,
            customerDtos
        );
    }

    public async Task<ListResultDto<AddressLookupDto>> GetAddressLookupAsync()
    {
        var addresses = await _addressRepository.GetListAsync();

        return new ListResultDto<AddressLookupDto>(
            ObjectMapper.Map<List<Address>, List<AddressLookupDto>>(addresses)
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
            return $"customer.{nameof(Customer.Id)}";
        }

        if (sorting.Contains("addressCity", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "addressCity",
                "Address.City",
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

        return $"customer.{sorting}";
    }
}
