using System.Threading.Tasks;
using CrmApp.ServiceCategories;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.ServiceCategories;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateUpdateServiceCategoryDto ServiceCategory { get; set; } = null!;

    private readonly IServiceCategoryAppService _serviceCategoryAppService;

    public CreateModalModel(IServiceCategoryAppService serviceCategoryAppService)
    {
        _serviceCategoryAppService = serviceCategoryAppService;
    }

    public void OnGet()
    {
        ServiceCategory = new CreateUpdateServiceCategoryDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _serviceCategoryAppService.CreateAsync(ServiceCategory);
        return NoContent();
    }
}
