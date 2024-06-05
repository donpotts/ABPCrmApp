using Xunit;

namespace CrmApp.EntityFrameworkCore;

[CollectionDefinition(CrmAppTestConsts.CollectionDefinitionName)]
public class CrmAppEntityFrameworkCoreCollection : ICollectionFixture<CrmAppEntityFrameworkCoreFixture>
{

}
