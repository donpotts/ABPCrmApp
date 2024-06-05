using System.Threading.Tasks;

namespace CrmApp.Data;

public interface ICrmAppDbSchemaMigrator
{
    Task MigrateAsync();
}
