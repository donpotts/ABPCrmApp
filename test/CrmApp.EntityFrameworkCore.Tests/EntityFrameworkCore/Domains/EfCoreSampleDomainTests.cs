using CrmApp.Samples;
using Xunit;

namespace CrmApp.EntityFrameworkCore.Domains;

[Collection(CrmAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<CrmAppEntityFrameworkCoreTestModule>
{

}
