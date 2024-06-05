using CrmApp.Samples;
using Xunit;

namespace CrmApp.EntityFrameworkCore.Applications;

[Collection(CrmAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<CrmAppEntityFrameworkCoreTestModule>
{

}
