using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CrmApp.ServiceCategories;
using CrmApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Services;

[Authorize(CrmAppPermissions.Services.Default)]
public class ServiceAppService :
    CrudAppService<
        Service, //The Service entity
        ServiceDto, //Used to show services
        int, //Primary key of the service entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateServiceDto>, //Used to create/update a service
    IServiceAppService //implement the IServiceAppService
{
    private readonly IRepository<ServiceCategory, int> _serviceCategoryRepository;

    public ServiceAppService(
        IRepository<Service, int> repository,
        IRepository<ServiceCategory, int> serviceCategoryRepository)
        : base(repository)
    {
        _serviceCategoryRepository = serviceCategoryRepository;
        GetPolicyName = CrmAppPermissions.Services.Default;
        GetListPolicyName = CrmAppPermissions.Services.Default;
        CreatePolicyName = CrmAppPermissions.Services.Create;
        UpdatePolicyName = CrmAppPermissions.Services.Edit;
        DeletePolicyName = CrmAppPermissions.Services.Delete;
    }

    public override async Task<ServiceDto> GetAsync(int id)
    {
        //Get the IQueryable<Service> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join services and serviceCategories
        var query = from service in queryable
            join serviceCategory in await _serviceCategoryRepository.GetQueryableAsync() on service.ServiceCategoryId equals serviceCategory.Id
            where service.Id == id
            select new { service, serviceCategory };

        //Execute the query and get the service with serviceCategory
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Service), id);
        }

        var serviceDto = ObjectMapper.Map<Service, ServiceDto>(queryResult.service);
        serviceDto.ServiceCategoryName = queryResult.serviceCategory.Name;
        return serviceDto;
    }

    public override async Task<PagedResultDto<ServiceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Service> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join services and serviceCategories
        var query = from service in queryable
            join serviceCategory in await _serviceCategoryRepository.GetQueryableAsync() on service.ServiceCategoryId equals serviceCategory.Id
            select new {service, serviceCategory};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of ServiceDto objects
        var serviceDtos = queryResult.Select(x =>
        {
            var serviceDto = ObjectMapper.Map<Service, ServiceDto>(x.service);
            serviceDto.ServiceCategoryName = x.serviceCategory.Name;
            return serviceDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<ServiceDto>(
            totalCount,
            serviceDtos
        );
    }

    public async Task<ListResultDto<ServiceCategoryLookupDto>> GetServiceCategoryLookupAsync()
    {
        var serviceCategories = await _serviceCategoryRepository.GetListAsync();

        return new ListResultDto<ServiceCategoryLookupDto>(
            ObjectMapper.Map<List<ServiceCategory>, List<ServiceCategoryLookupDto>>(serviceCategories)
        );
    }

    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"service.{nameof(Service.Id)}";
        }

        if (sorting.Contains("serviceCategoryName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "serviceCategoryName",
                "ServiceCategory.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"service.{sorting}";
    }
}
