using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CrmApp.Data;

/* This is used if database provider does't define
 * ICrmAppDbSchemaMigrator implementation.
 */
public class NullCrmAppDbSchemaMigrator : ICrmAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
