using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace CrmApp.Web;

[Dependency(ReplaceServices = true)]
public class CrmAppBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "CRM App";
}
