using System.Threading.Tasks;
using CrmApp.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.Addresses;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateUpdateAddressDto Address { get; set; } = null!;

    private readonly IAddressAppService _addressAppService;

    public CreateModalModel(IAddressAppService addressAppService)
    {
        _addressAppService = addressAppService;
    }

    public void OnGet()
    {
        Address = new CreateUpdateAddressDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _addressAppService.CreateAsync(Address);
        return NoContent();
    }
}
