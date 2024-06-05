using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using CrmApp.ProductCategories;
using CrmApp.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace CrmApp.Products;

[Authorize(CrmAppPermissions.Products.Default)]
public class ProductAppService :
    CrudAppService<
        Product, //The Product entity
        ProductDto, //Used to show products
        int, //Primary key of the product entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProductDto>, //Used to create/update a product
    IProductAppService //implement the IProductAppService
{
    private readonly IRepository<ProductCategory, int> _productCategoryRepository;

    public ProductAppService(
        IRepository<Product, int> repository,
        IRepository<ProductCategory, int> productCategoryRepository)
        : base(repository)
    {
        _productCategoryRepository = productCategoryRepository;
        GetPolicyName = CrmAppPermissions.Products.Default;
        GetListPolicyName = CrmAppPermissions.Products.Default;
        CreatePolicyName = CrmAppPermissions.Products.Create;
        UpdatePolicyName = CrmAppPermissions.Products.Edit;
        DeletePolicyName = CrmAppPermissions.Products.Delete;
    }

    public override async Task<ProductDto> GetAsync(int id)
    {
        //Get the IQueryable<Product> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join products and productCategories
        var query = from product in queryable
            join productCategory in await _productCategoryRepository.GetQueryableAsync() on product.ProductCategoryId equals productCategory.Id
            where product.Id == id
            select new { product, productCategory };

        //Execute the query and get the product with productCategory
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Product), id);
        }

        var productDto = ObjectMapper.Map<Product, ProductDto>(queryResult.product);
        productDto.ProductCategoryName = queryResult.productCategory.Name;
        return productDto;
    }

    public override async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Product> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join products and productCategories
        var query = from product in queryable
            join productCategory in await _productCategoryRepository.GetQueryableAsync() on product.ProductCategoryId equals productCategory.Id
            select new {product, productCategory};

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of ProductDto objects
        var productDtos = queryResult.Select(x =>
        {
            var productDto = ObjectMapper.Map<Product, ProductDto>(x.product);
            productDto.ProductCategoryName = x.productCategory.Name;
            return productDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<ProductDto>(
            totalCount,
            productDtos
        );
    }

    public async Task<ListResultDto<ProductCategoryLookupDto>> GetProductCategoryLookupAsync()
    {
        var productCategories = await _productCategoryRepository.GetListAsync();

        return new ListResultDto<ProductCategoryLookupDto>(
            ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryLookupDto>>(productCategories)
        );
    }

    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"product.{nameof(Product.Id)}";
        }

        if (sorting.Contains("productCategoryName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "productCategoryName",
                "ProductCategory.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"product.{sorting}";
    }
}
