using System;
using System.Threading.Tasks;
using CrmApp.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace CrmApp.Web.Pages.Addresses;

public class EditModalModel : CrmAppPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public CreateUpdateAddressDto Address { get; set; } = null!;

    private readonly IAddressAppService _addressAppService;

    public EditModalModel(IAddressAppService addressAppService)
    {
        _addressAppService = addressAppService;
    }

    public async Task OnGetAsync()
    {
        var addressDto = await _addressAppService.GetAsync(Id);
        Address = ObjectMapper.Map<AddressDto, CreateUpdateAddressDto>(addressDto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _addressAppService.UpdateAsync(Id, Address);
        return NoContent();
    }
}
