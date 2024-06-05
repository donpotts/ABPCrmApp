using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CrmApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CrmApp.Web.Pages.Services;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateServiceViewModel Service { get; set; } = null!;

    public List<SelectListItem> ServiceCategories { get; set; } = null!;

    private readonly IServiceAppService _serviceAppService;

    public CreateModalModel(IServiceAppService serviceAppService)
    {
        _serviceAppService = serviceAppService;
    }

    public async Task OnGetAsync()
    {
        Service = new CreateServiceViewModel();

        var serviceCategoryLookup = await _serviceAppService.GetServiceCategoryLookupAsync();
        ServiceCategories = serviceCategoryLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _serviceAppService.CreateAsync(
            ObjectMapper.Map<CreateServiceViewModel, CreateUpdateServiceDto>(Service)
            );
        return NoContent();
    }

    public class CreateServiceViewModel
    {
 
        [SelectItems(nameof(ServiceCategories))]
        [DisplayName("ServiceCategory")]
        public int ServiceCategoryId { get; set; }

 
        public string? Name { get; set; }

 
        public string? Description { get; set; }

 
        public string? Recurring { get; set; }

 
        public string? Icon { get; set; }

 
        public string? Notes { get; set; }
    }
}
