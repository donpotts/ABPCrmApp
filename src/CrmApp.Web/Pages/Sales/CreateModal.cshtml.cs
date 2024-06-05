using System.Threading.Tasks;
using CrmApp.Sales;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.Sales;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateUpdateSaleDto Sale { get; set; } = null!;

    private readonly ISaleAppService _saleAppService;

    public CreateModalModel(ISaleAppService saleAppService)
    {
        _saleAppService = saleAppService;
    }

    public void OnGet()
    {
        Sale = new CreateUpdateSaleDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _saleAppService.CreateAsync(Sale);
        return NoContent();
    }
}
