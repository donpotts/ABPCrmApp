using CrmApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CrmApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CrmAppEntityFrameworkCoreModule),
    typeof(CrmAppApplicationContractsModule)
    )]
public class CrmAppDbMigratorModule : AbpModule
{
}
