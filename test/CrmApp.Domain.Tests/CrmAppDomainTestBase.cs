using Volo.Abp.Modularity;

namespace CrmApp;

/* Inherit from this class for your domain layer tests. */
public abstract class CrmAppDomainTestBase<TStartupModule> : CrmAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
