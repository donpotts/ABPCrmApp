using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CrmApp.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace CrmApp.Web.Pages.Contacts;

public class EditModalModel : CrmAppPageModel
{

    [BindProperty]
    public EditContactViewModel Contact { get; set; } = null!;

    public List<SelectListItem> Addresses { get; set; } = null!;
    public List<SelectListItem> Rewards { get; set; } = null!;

    private readonly IContactAppService _contactAppService;

    public EditModalModel(IContactAppService contactAppService)
    {
        _contactAppService = contactAppService;
    }

    public async Task OnGetAsync(int id)
    {
        var contactDto = await _contactAppService.GetAsync(id);
        Contact = ObjectMapper.Map<ContactDto, EditContactViewModel>(contactDto);

        var addressLookup = await _contactAppService.GetAddressLookupAsync();
        Addresses = addressLookup.Items
            .Select(x => new SelectListItem(x.City, x.Id.ToString()))
            .ToList();

        var rewardLookup = await _contactAppService.GetRewardLookupAsync();
        Rewards = rewardLookup.Items
            .Select(x => new SelectListItem(x.Rewardpoints.ToString(), x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _contactAppService.UpdateAsync(
            Contact.Id,
            ObjectMapper.Map<EditContactViewModel, CreateUpdateContactDto>(Contact)
        );

        return NoContent();
    }

    public class EditContactViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

 
        public string? Name { get; set; }

 
        public string? Email { get; set; }

 
        public int Phone { get; set; }

 
        public string? Role { get; set; }

 
        [SelectItems(nameof(Addresses))]
        [DisplayName("Address")]
        public int AddressId { get; set; }

 
        public string? Photo { get; set; }

 
        [SelectItems(nameof(Rewards))]
        [DisplayName("Reward")]
        public int RewardId { get; set; }

 
        public string? Notes { get; set; }
    }
}
