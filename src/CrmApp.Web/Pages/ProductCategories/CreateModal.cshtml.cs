using System.Threading.Tasks;
using CrmApp.ProductCategories;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.ProductCategories;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateUpdateProductCategoryDto ProductCategory { get; set; } = null!;

    private readonly IProductCategoryAppService _productCategoryAppService;

    public CreateModalModel(IProductCategoryAppService productCategoryAppService)
    {
        _productCategoryAppService = productCategoryAppService;
    }

    public void OnGet()
    {
        ProductCategory = new CreateUpdateProductCategoryDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _productCategoryAppService.CreateAsync(ProductCategory);
        return NoContent();
    }
}
