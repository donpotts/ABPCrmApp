using System;
using System.Threading.Tasks;
using CrmApp.ServiceCategories;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.ServiceCategories;

public class EditModalModel : CrmAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public CreateUpdateServiceCategoryDto ServiceCategory { get; set; } = null!;

    private readonly IServiceCategoryAppService _serviceCategoryAppService;

    public EditModalModel(IServiceCategoryAppService serviceCategoryAppService)
    {
        _serviceCategoryAppService = serviceCategoryAppService;
    }

    public async Task OnGetAsync()
    {
        var serviceCategoryDto = await _serviceCategoryAppService.GetAsync(Id);
        ServiceCategory = ObjectMapper.Map<ServiceCategoryDto, CreateUpdateServiceCategoryDto>(serviceCategoryDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _serviceCategoryAppService.UpdateAsync(Id, ServiceCategory);
        return NoContent();
    }
}
