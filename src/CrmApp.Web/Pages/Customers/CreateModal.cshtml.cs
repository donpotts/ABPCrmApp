using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CrmApp.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CrmApp.Web.Pages.Customers;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateCustomerViewModel Customer { get; set; } = null!;

    public List<SelectListItem> Addresses { get; set; } = null!;
    public List<SelectListItem> Contacts { get; set; } = null!;

    private readonly ICustomerAppService _customerAppService;

    public CreateModalModel(ICustomerAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    public async Task OnGetAsync()
    {
        Customer = new CreateCustomerViewModel();

        var addressLookup = await _customerAppService.GetAddressLookupAsync();
        Addresses = addressLookup.Items
            .Select(x => new SelectListItem(x.City, x.Id.ToString()))
            .ToList();

        var contactLookup = await _customerAppService.GetContactLookupAsync();
        Contacts = contactLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _customerAppService.CreateAsync(
            ObjectMapper.Map<CreateCustomerViewModel, CreateUpdateCustomerDto>(Customer)
            );
        return NoContent();
    }

    public class CreateCustomerViewModel
    {
 
        public string? Name { get; set; }

 
        public string? Type { get; set; }

 
        public string? Industry { get; set; }

 
        [SelectItems(nameof(Addresses))]
        [DisplayName("Address")]
        public int AddressId { get; set; }

 
        [SelectItems(nameof(Contacts))]
        [DisplayName("Contact")]
        public int ContactId { get; set; }

 
        public string? Logo { get; set; }

 
        public string? Notes { get; set; }
    }
}
