using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CrmApp.EntityFrameworkCore;
using CrmApp.Localization;
using CrmApp.MultiTenancy;
using CrmApp.Permissions;
using CrmApp.Web.Menus;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.OpenIddict;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace CrmApp.Web;

[DependsOn(
    typeof(CrmAppHttpApiModule),
    typeof(CrmAppApplicationModule),
    typeof(CrmAppEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class CrmAppWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray() ?? []
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(CrmAppResource),
                typeof(CrmAppDomainModule).Assembly,
                typeof(CrmAppDomainSharedModule).Assembly,
                typeof(CrmAppApplicationModule).Assembly,
                typeof(CrmAppApplicationContractsModule).Assembly,
                typeof(CrmAppWebModule).Assembly
            );
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("CrmApp");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "2d2e7d44-3686-4b0b-9e00-9712b232a062");
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);

            options.Conventions.AuthorizePage("/Services/Index", CrmAppPermissions.Services.Default);
            options.Conventions.AuthorizePage("/Services/CreateModal", CrmAppPermissions.Services.Create);
            options.Conventions.AuthorizePage("/Services/EditModal", CrmAppPermissions.Services.Edit);

            options.Conventions.AuthorizePage("/Sales/Index", CrmAppPermissions.Sales.Default);
            options.Conventions.AuthorizePage("/Sales/CreateModal", CrmAppPermissions.Sales.Create);
            options.Conventions.AuthorizePage("/Sales/EditModal", CrmAppPermissions.Sales.Edit);

            options.Conventions.AuthorizePage("/Vendors/Index", CrmAppPermissions.Vendors.Default);
            options.Conventions.AuthorizePage("/Vendors/CreateModal", CrmAppPermissions.Vendors.Create);
            options.Conventions.AuthorizePage("/Vendors/EditModal", CrmAppPermissions.Vendors.Edit);

            options.Conventions.AuthorizePage("/SupportCases/Index", CrmAppPermissions.SupportCases.Default);
            options.Conventions.AuthorizePage("/SupportCases/CreateModal", CrmAppPermissions.SupportCases.Create);
            options.Conventions.AuthorizePage("/SupportCases/EditModal", CrmAppPermissions.SupportCases.Edit);

            options.Conventions.AuthorizePage("/TodoTasks/Index", CrmAppPermissions.TodoTasks.Default);
            options.Conventions.AuthorizePage("/TodoTasks/CreateModal", CrmAppPermissions.TodoTasks.Create);
            options.Conventions.AuthorizePage("/TodoTasks/EditModal", CrmAppPermissions.TodoTasks.Edit);

            options.Conventions.AuthorizePage("/Rewards/Index", CrmAppPermissions.Rewards.Default);
            options.Conventions.AuthorizePage("/Rewards/CreateModal", CrmAppPermissions.Rewards.Create);
            options.Conventions.AuthorizePage("/Rewards/EditModal", CrmAppPermissions.Rewards.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);

            options.Conventions.AuthorizePage("/Services/Index", CrmAppPermissions.Services.Default);
            options.Conventions.AuthorizePage("/Services/CreateModal", CrmAppPermissions.Services.Create);
            options.Conventions.AuthorizePage("/Services/EditModal", CrmAppPermissions.Services.Edit);

            options.Conventions.AuthorizePage("/Sales/Index", CrmAppPermissions.Sales.Default);
            options.Conventions.AuthorizePage("/Sales/CreateModal", CrmAppPermissions.Sales.Create);
            options.Conventions.AuthorizePage("/Sales/EditModal", CrmAppPermissions.Sales.Edit);

            options.Conventions.AuthorizePage("/Vendors/Index", CrmAppPermissions.Vendors.Default);
            options.Conventions.AuthorizePage("/Vendors/CreateModal", CrmAppPermissions.Vendors.Create);
            options.Conventions.AuthorizePage("/Vendors/EditModal", CrmAppPermissions.Vendors.Edit);

            options.Conventions.AuthorizePage("/SupportCases/Index", CrmAppPermissions.SupportCases.Default);
            options.Conventions.AuthorizePage("/SupportCases/CreateModal", CrmAppPermissions.SupportCases.Create);
            options.Conventions.AuthorizePage("/SupportCases/EditModal", CrmAppPermissions.SupportCases.Edit);

            options.Conventions.AuthorizePage("/TodoTasks/Index", CrmAppPermissions.TodoTasks.Default);
            options.Conventions.AuthorizePage("/TodoTasks/CreateModal", CrmAppPermissions.TodoTasks.Create);
            options.Conventions.AuthorizePage("/TodoTasks/EditModal", CrmAppPermissions.TodoTasks.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);

            options.Conventions.AuthorizePage("/Services/Index", CrmAppPermissions.Services.Default);
            options.Conventions.AuthorizePage("/Services/CreateModal", CrmAppPermissions.Services.Create);
            options.Conventions.AuthorizePage("/Services/EditModal", CrmAppPermissions.Services.Edit);

            options.Conventions.AuthorizePage("/Sales/Index", CrmAppPermissions.Sales.Default);
            options.Conventions.AuthorizePage("/Sales/CreateModal", CrmAppPermissions.Sales.Create);
            options.Conventions.AuthorizePage("/Sales/EditModal", CrmAppPermissions.Sales.Edit);

            options.Conventions.AuthorizePage("/Vendors/Index", CrmAppPermissions.Vendors.Default);
            options.Conventions.AuthorizePage("/Vendors/CreateModal", CrmAppPermissions.Vendors.Create);
            options.Conventions.AuthorizePage("/Vendors/EditModal", CrmAppPermissions.Vendors.Edit);

            options.Conventions.AuthorizePage("/SupportCases/Index", CrmAppPermissions.SupportCases.Default);
            options.Conventions.AuthorizePage("/SupportCases/CreateModal", CrmAppPermissions.SupportCases.Create);
            options.Conventions.AuthorizePage("/SupportCases/EditModal", CrmAppPermissions.SupportCases.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);

            options.Conventions.AuthorizePage("/Services/Index", CrmAppPermissions.Services.Default);
            options.Conventions.AuthorizePage("/Services/CreateModal", CrmAppPermissions.Services.Create);
            options.Conventions.AuthorizePage("/Services/EditModal", CrmAppPermissions.Services.Edit);

            options.Conventions.AuthorizePage("/Sales/Index", CrmAppPermissions.Sales.Default);
            options.Conventions.AuthorizePage("/Sales/CreateModal", CrmAppPermissions.Sales.Create);
            options.Conventions.AuthorizePage("/Sales/EditModal", CrmAppPermissions.Sales.Edit);

            options.Conventions.AuthorizePage("/Vendors/Index", CrmAppPermissions.Vendors.Default);
            options.Conventions.AuthorizePage("/Vendors/CreateModal", CrmAppPermissions.Vendors.Create);
            options.Conventions.AuthorizePage("/Vendors/EditModal", CrmAppPermissions.Vendors.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);

            options.Conventions.AuthorizePage("/Services/Index", CrmAppPermissions.Services.Default);
            options.Conventions.AuthorizePage("/Services/CreateModal", CrmAppPermissions.Services.Create);
            options.Conventions.AuthorizePage("/Services/EditModal", CrmAppPermissions.Services.Edit);

            options.Conventions.AuthorizePage("/Sales/Index", CrmAppPermissions.Sales.Default);
            options.Conventions.AuthorizePage("/Sales/CreateModal", CrmAppPermissions.Sales.Create);
            options.Conventions.AuthorizePage("/Sales/EditModal", CrmAppPermissions.Sales.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);

            options.Conventions.AuthorizePage("/Services/Index", CrmAppPermissions.Services.Default);
            options.Conventions.AuthorizePage("/Services/CreateModal", CrmAppPermissions.Services.Create);
            options.Conventions.AuthorizePage("/Services/EditModal", CrmAppPermissions.Services.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);

            options.Conventions.AuthorizePage("/Products/Index", CrmAppPermissions.Products.Default);
            options.Conventions.AuthorizePage("/Products/CreateModal", CrmAppPermissions.Products.Create);
            options.Conventions.AuthorizePage("/Products/EditModal", CrmAppPermissions.Products.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);

            options.Conventions.AuthorizePage("/Leads/Index", CrmAppPermissions.Leads.Default);
            options.Conventions.AuthorizePage("/Leads/CreateModal", CrmAppPermissions.Leads.Create);
            options.Conventions.AuthorizePage("/Leads/EditModal", CrmAppPermissions.Leads.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);

            options.Conventions.AuthorizePage("/Opportunities/Index", CrmAppPermissions.Opportunities.Default);
            options.Conventions.AuthorizePage("/Opportunities/CreateModal", CrmAppPermissions.Opportunities.Create);
            options.Conventions.AuthorizePage("/Opportunities/EditModal", CrmAppPermissions.Opportunities.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);

            options.Conventions.AuthorizePage("/Contacts/Index", CrmAppPermissions.Contacts.Default);
            options.Conventions.AuthorizePage("/Contacts/CreateModal", CrmAppPermissions.Contacts.Create);
            options.Conventions.AuthorizePage("/Contacts/EditModal", CrmAppPermissions.Contacts.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);

            options.Conventions.AuthorizePage("/ServiceCategories/Index", CrmAppPermissions.ServiceCategories.Default);
            options.Conventions.AuthorizePage("/ServiceCategories/CreateModal", CrmAppPermissions.ServiceCategories.Create);
            options.Conventions.AuthorizePage("/ServiceCategories/EditModal", CrmAppPermissions.ServiceCategories.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);

            options.Conventions.AuthorizePage("/ProductCategories/Index", CrmAppPermissions.ProductCategories.Default);
            options.Conventions.AuthorizePage("/ProductCategories/CreateModal", CrmAppPermissions.ProductCategories.Create);
            options.Conventions.AuthorizePage("/ProductCategories/EditModal", CrmAppPermissions.ProductCategories.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);

            options.Conventions.AuthorizePage("/Addresses/Index", CrmAppPermissions.Addresses.Default);
            options.Conventions.AuthorizePage("/Addresses/CreateModal", CrmAppPermissions.Addresses.Create);
            options.Conventions.AuthorizePage("/Addresses/EditModal", CrmAppPermissions.Addresses.Edit);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Customers/Index", CrmAppPermissions.Customers.Default);
            options.Conventions.AuthorizePage("/Customers/CreateModal", CrmAppPermissions.Customers.Create);
            options.Conventions.AuthorizePage("/Customers/EditModal", CrmAppPermissions.Customers.Edit);
        });

        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CrmAppWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<CrmAppDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}CrmApp.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<CrmAppDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}CrmApp.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<CrmAppApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}CrmApp.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<CrmAppApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}CrmApp.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<CrmAppWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CrmAppMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(CrmAppApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM App API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseCors();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM App API");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
