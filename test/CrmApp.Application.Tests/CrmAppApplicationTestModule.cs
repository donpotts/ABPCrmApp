using Volo.Abp.Modularity;

namespace CrmApp;

[DependsOn(
    typeof(CrmAppApplicationModule),
    typeof(CrmAppDomainTestModule)
)]
public class CrmAppApplicationTestModule : AbpModule
{

}
