using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CrmApp.Leads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CrmApp.Web.Pages.Leads;

public class CreateModalModel : CrmAppPageModel
{
    [BindProperty]
    public CreateLeadViewModel Lead { get; set; } = null!;

    public List<SelectListItem> Addresses { get; set; } = null!;
    public List<SelectListItem> Opportunities { get; set; } = null!;
    public List<SelectListItem> Contacts { get; set; } = null!;

    private readonly ILeadAppService _leadAppService;

    public CreateModalModel(ILeadAppService leadAppService)
    {
        _leadAppService = leadAppService;
    }

    public async Task OnGetAsync()
    {
        Lead = new CreateLeadViewModel();

        var addressLookup = await _leadAppService.GetAddressLookupAsync();
        Addresses = addressLookup.Items
            .Select(x => new SelectListItem(x.City, x.Id.ToString()))
            .ToList();

        var opportunityLookup = await _leadAppService.GetOpportunityLookupAsync();
        Opportunities = opportunityLookup.Items
            .Select(x => new SelectListItem(x.Stage, x.Id.ToString()))
            .ToList();

        var contactLookup = await _leadAppService.GetContactLookupAsync();
        Contacts = contactLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _leadAppService.CreateAsync(
            ObjectMapper.Map<CreateLeadViewModel, CreateUpdateLeadDto>(Lead)
            );
        return NoContent();
    }

    public class CreateLeadViewModel
    {
 
        [SelectItems(nameof(Contacts))]
        [DisplayName("Contact")]
        public int ContactId { get; set; }

 
        public string? Source { get; set; }

 
        public string? Status { get; set; }

 
        public double PotentialValue { get; set; }

 
        [SelectItems(nameof(Opportunities))]
        [DisplayName("Opportunity")]
        public int OpportunityId { get; set; }

 
        [SelectItems(nameof(Addresses))]
        [DisplayName("Address")]
        public int AddressId { get; set; }

 
        public string? Photo { get; set; }

 
        public string? Notes { get; set; }
    }
}
