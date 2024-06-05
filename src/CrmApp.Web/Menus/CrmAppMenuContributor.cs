using System.Threading.Tasks;
using CrmApp.Localization;
using CrmApp.MultiTenancy;
using CrmApp.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace CrmApp.Web.Menus;

public class CrmAppMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<CrmAppResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                CrmAppMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "CrmApp",
                l["Menu:CrmApp"],
                icon: "fa fa-database")
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Customers",
                    l["Menu:Customers"],
                    url: "/Customers"
                ).RequirePermissions(CrmAppPermissions.Customers.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Addresses",
                    l["Menu:Addresses"],
                    url: "/Addresses"
                ).RequirePermissions(CrmAppPermissions.Addresses.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.ProductCategories",
                    l["Menu:ProductCategories"],
                    url: "/ProductCategories"
                ).RequirePermissions(CrmAppPermissions.ProductCategories.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.ServiceCategories",
                    l["Menu:ServiceCategories"],
                    url: "/ServiceCategories"
                ).RequirePermissions(CrmAppPermissions.ServiceCategories.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Contacts",
                    l["Menu:Contacts"],
                    url: "/Contacts"
                ).RequirePermissions(CrmAppPermissions.Contacts.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Opportunities",
                    l["Menu:Opportunities"],
                    url: "/Opportunities"
                ).RequirePermissions(CrmAppPermissions.Opportunities.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Leads",
                    l["Menu:Leads"],
                    url: "/Leads"
                ).RequirePermissions(CrmAppPermissions.Leads.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Products",
                    l["Menu:Products"],
                    url: "/Products"
                ).RequirePermissions(CrmAppPermissions.Products.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Services",
                    l["Menu:Services"],
                    url: "/Services"
                ).RequirePermissions(CrmAppPermissions.Services.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Sales",
                    l["Menu:Sales"],
                    url: "/Sales"
                ).RequirePermissions(CrmAppPermissions.Sales.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Vendors",
                    l["Menu:Vendors"],
                    url: "/Vendors"
                ).RequirePermissions(CrmAppPermissions.Vendors.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.SupportCases",
                    l["Menu:SupportCases"],
                    url: "/SupportCases"
                ).RequirePermissions(CrmAppPermissions.SupportCases.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.TodoTasks",
                    l["Menu:TodoTasks"],
                    url: "/TodoTasks"
                ).RequirePermissions(CrmAppPermissions.TodoTasks.Default) // Check the permission!
            )
            .AddItem(
                new ApplicationMenuItem(
                    "CrmApp.Rewards",
                    l["Menu:Rewards"],
                    url: "/Rewards"
                ).RequirePermissions(CrmAppPermissions.Rewards.Default) // Check the permission!
            )
        );

        return Task.CompletedTask;
    }
}
