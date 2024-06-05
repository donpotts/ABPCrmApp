using System;
using System.Threading.Tasks;
using CrmApp.Sales;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.Sales;

public class EditModalModel : CrmAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public CreateUpdateSaleDto Sale { get; set; } = null!;

    private readonly ISaleAppService _saleAppService;

    public EditModalModel(ISaleAppService saleAppService)
    {
        _saleAppService = saleAppService;
    }

    public async Task OnGetAsync()
    {
        var saleDto = await _saleAppService.GetAsync(Id);
        Sale = ObjectMapper.Map<SaleDto, CreateUpdateSaleDto>(saleDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _saleAppService.UpdateAsync(Id, Sale);
        return NoContent();
    }
}
