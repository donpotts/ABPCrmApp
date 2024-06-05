using Volo.Abp.Modularity;

namespace CrmApp;

public abstract class CrmAppApplicationTestBase<TStartupModule> : CrmAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
