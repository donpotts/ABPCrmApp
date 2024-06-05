using Microsoft.AspNetCore.Builder;
using CrmApp;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<CrmAppWebTestModule>();

public partial class Program
{
}
