using Volo.Abp.Modularity;

namespace CrmApp;

[DependsOn(
    typeof(CrmAppDomainModule),
    typeof(CrmAppTestBaseModule)
)]
public class CrmAppDomainTestModule : AbpModule
{

}
