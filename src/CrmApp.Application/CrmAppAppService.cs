using System;
using System.Collections.Generic;
using System.Text;
using CrmApp.Localization;
using Volo.Abp.Application.Services;

namespace CrmApp;

/* Inherit your application services from this class.
 */
public abstract class CrmAppAppService : ApplicationService
{
    protected CrmAppAppService()
    {
        LocalizationResource = typeof(CrmAppResource);
    }
}
