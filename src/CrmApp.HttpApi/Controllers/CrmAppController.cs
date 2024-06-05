using CrmApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace CrmApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CrmAppController : AbpControllerBase
{
    protected CrmAppController()
    {
        LocalizationResource = typeof(CrmAppResource);
    }
}
