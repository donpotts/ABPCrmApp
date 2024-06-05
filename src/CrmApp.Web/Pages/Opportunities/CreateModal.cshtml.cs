using System.Threading.Tasks;
using CrmApp.Opportunities;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.Opportunities;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateUpdateOpportunityDto Opportunity { get; set; } = null!;

    private readonly IOpportunityAppService _opportunityAppService;

    public CreateModalModel(IOpportunityAppService opportunityAppService)
    {
        _opportunityAppService = opportunityAppService;
    }

    public void OnGet()
    {
        Opportunity = new CreateUpdateOpportunityDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _opportunityAppService.CreateAsync(Opportunity);
        return NoContent();
    }
}
