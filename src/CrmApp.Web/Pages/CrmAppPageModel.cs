using CrmApp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace CrmApp.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class CrmAppPageModel : AbpPageModel
{
    protected CrmAppPageModel()
    {
        LocalizationResourceType = typeof(CrmAppResource);
    }
}
