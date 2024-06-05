using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CrmApp.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CrmApp.Web.Pages.Vendors;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateVendorViewModel Vendor { get; set; } = null!;

    public List<SelectListItem> Addresses { get; set; } = null!;
    public List<SelectListItem> Services { get; set; } = null!;
    public List<SelectListItem> Products { get; set; } = null!;

    private readonly IVendorAppService _vendorAppService;

    public CreateModalModel(IVendorAppService vendorAppService)
    {
        _vendorAppService = vendorAppService;
    }

    public async Task OnGetAsync()
    {
        Vendor = new CreateVendorViewModel();

        var addressLookup = await _vendorAppService.GetAddressLookupAsync();
        Addresses = addressLookup.Items
            .Select(x => new SelectListItem(x.City, x.Id.ToString()))
            .ToList();

        var serviceLookup = await _vendorAppService.GetServiceLookupAsync();
        Services = serviceLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();

        var productLookup = await _vendorAppService.GetProductLookupAsync();
        Products = productLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _vendorAppService.CreateAsync(
            ObjectMapper.Map<CreateVendorViewModel, CreateUpdateVendorDto>(Vendor)
            );
        return NoContent();
    }

    public class CreateVendorViewModel
    {
 
        public string? Name { get; set; }

 
        public string? ContactName { get; set; }

 
        public int Phone { get; set; }

 
        public string? Email { get; set; }

 
        [SelectItems(nameof(Addresses))]
        [DisplayName("Address")]
        public int AddressId { get; set; }

 
        [SelectItems(nameof(Products))]
        [DisplayName("Product")]
        public int ProductId { get; set; }

 
        [SelectItems(nameof(Services))]
        [DisplayName("Service")]
        public int ServiceId { get; set; }

 
        public string? Logo { get; set; }

 
        public string? Notes { get; set; }
    }
}
