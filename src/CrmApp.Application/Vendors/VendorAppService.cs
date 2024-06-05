using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CrmApp.Addresses;
using CrmApp.Services;
using CrmApp.Products;
using CrmApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Vendors;

[Authorize(CrmAppPermissions.Vendors.Default)]
public class VendorAppService :
    CrudAppService<
        Vendor, //The Vendor entity
        VendorDto, //Used to show vendors
        int, //Primary key of the vendor entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateVendorDto>, //Used to create/update a vendor
    IVendorAppService //implement the IVendorAppService
{
    private readonly IRepository<Address, int> _addressRepository;
    private readonly IRepository<Service, int> _serviceRepository;
    private readonly IRepository<Product, int> _productRepository;

    public VendorAppService(
        IRepository<Vendor, int> repository,
        IRepository<Address, int> addressRepository,
        IRepository<Service, int> serviceRepository,
        IRepository<Product, int> productRepository)
        : base(repository)
    {
        _addressRepository = addressRepository;
        _serviceRepository = serviceRepository;
        _productRepository = productRepository;
        GetPolicyName = CrmAppPermissions.Vendors.Default;
        GetListPolicyName = CrmAppPermissions.Vendors.Default;
        CreatePolicyName = CrmAppPermissions.Vendors.Create;
        UpdatePolicyName = CrmAppPermissions.Vendors.Edit;
        DeletePolicyName = CrmAppPermissions.Vendors.Delete;
    }

    public override async Task<VendorDto> GetAsync(int id)
    {
        //Get the IQueryable<Vendor> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join vendors and addresses, services, products
        var query = from vendor in queryable
            join address in await _addressRepository.GetQueryableAsync() on vendor.AddressId equals address.Id
            join service in await _serviceRepository.GetQueryableAsync() on vendor.ServiceId equals service.Id
            join product in await _productRepository.GetQueryableAsync() on vendor.ProductId equals product.Id
            where vendor.Id == id
            select new { vendor, address, service, product };

        //Execute the query and get the vendor with address, service, product
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Vendor), id);
        }

        var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(queryResult.vendor);
        vendorDto.AddressCity = queryResult.address.City;
        vendorDto.ServiceName = queryResult.service.Name;
        vendorDto.ProductName = queryResult.product.Name;
        return vendorDto;
    }

    public override async Task<PagedResultDto<VendorDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Vendor> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join vendors and addresses, services, products
        var query = from vendor in queryable
            join address in await _addressRepository.GetQueryableAsync() on vendor.AddressId equals address.Id
            join service in await _serviceRepository.GetQueryableAsync() on vendor.ServiceId equals service.Id
            join product in await _productRepository.GetQueryableAsync() on vendor.ProductId equals product.Id
            select new {vendor, address, service, product};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of VendorDto objects
        var vendorDtos = queryResult.Select(x =>
        {
            var vendorDto = ObjectMapper.Map<Vendor, VendorDto>(x.vendor);
            vendorDto.AddressCity = x.address.City;
            vendorDto.ServiceName = x.service.Name;
            vendorDto.ProductName = x.product.Name;
            return vendorDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<VendorDto>(
            totalCount,
            vendorDtos
        );
    }

    public async Task<ListResultDto<AddressLookupDto>> GetAddressLookupAsync()
    {
        var addresses = await _addressRepository.GetListAsync();

        return new ListResultDto<AddressLookupDto>(
            ObjectMapper.Map<List<Address>, List<AddressLookupDto>>(addresses)
        );
    }

    public async Task<ListResultDto<ServiceLookupDto>> GetServiceLookupAsync()
    {
        var services = await _serviceRepository.GetListAsync();

        return new ListResultDto<ServiceLookupDto>(
            ObjectMapper.Map<List<Service>, List<ServiceLookupDto>>(services)
        );
    }

    public async Task<ListResultDto<ProductLookupDto>> GetProductLookupAsync()
    {
        var products = await _productRepository.GetListAsync();

        return new ListResultDto<ProductLookupDto>(
            ObjectMapper.Map<List<Product>, List<ProductLookupDto>>(products)
        );
    }

    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"vendor.{nameof(Vendor.Id)}";
        }

        if (sorting.Contains("addressCity", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "addressCity",
                "Address.City",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("serviceName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "serviceName",
                "Service.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("productName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "productName",
                "Product.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"vendor.{sorting}";
    }
}
