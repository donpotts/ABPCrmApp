using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace CrmApp.Pages;

public class Index_Tests : CrmAppWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
