using CrmApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CrmApp.Permissions;

public class CrmAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var crmAppGroup = context.AddGroup(CrmAppPermissions.GroupName, L("Permission:CrmApp"));

        var customersPermission = crmAppGroup.AddPermission(CrmAppPermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(CrmAppPermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(CrmAppPermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(CrmAppPermissions.Customers.Delete, L("Permission:Customers.Delete"));
        var addressesPermission = crmAppGroup.AddPermission(CrmAppPermissions.Addresses.Default, L("Permission:Addresses"));
        addressesPermission.AddChild(CrmAppPermissions.Addresses.Create, L("Permission:Addresses.Create"));
        addressesPermission.AddChild(CrmAppPermissions.Addresses.Edit, L("Permission:Addresses.Edit"));
        addressesPermission.AddChild(CrmAppPermissions.Addresses.Delete, L("Permission:Addresses.Delete"));
        var productCategoriesPermission = crmAppGroup.AddPermission(CrmAppPermissions.ProductCategories.Default, L("Permission:ProductCategories"));
        productCategoriesPermission.AddChild(CrmAppPermissions.ProductCategories.Create, L("Permission:ProductCategories.Create"));
        productCategoriesPermission.AddChild(CrmAppPermissions.ProductCategories.Edit, L("Permission:ProductCategories.Edit"));
        productCategoriesPermission.AddChild(CrmAppPermissions.ProductCategories.Delete, L("Permission:ProductCategories.Delete"));
        var serviceCategoriesPermission = crmAppGroup.AddPermission(CrmAppPermissions.ServiceCategories.Default, L("Permission:ServiceCategories"));
        serviceCategoriesPermission.AddChild(CrmAppPermissions.ServiceCategories.Create, L("Permission:ServiceCategories.Create"));
        serviceCategoriesPermission.AddChild(CrmAppPermissions.ServiceCategories.Edit, L("Permission:ServiceCategories.Edit"));
        serviceCategoriesPermission.AddChild(CrmAppPermissions.ServiceCategories.Delete, L("Permission:ServiceCategories.Delete"));
        var contactsPermission = crmAppGroup.AddPermission(CrmAppPermissions.Contacts.Default, L("Permission:Contacts"));
        contactsPermission.AddChild(CrmAppPermissions.Contacts.Create, L("Permission:Contacts.Create"));
        contactsPermission.AddChild(CrmAppPermissions.Contacts.Edit, L("Permission:Contacts.Edit"));
        contactsPermission.AddChild(CrmAppPermissions.Contacts.Delete, L("Permission:Contacts.Delete"));
        var opportunitiesPermission = crmAppGroup.AddPermission(CrmAppPermissions.Opportunities.Default, L("Permission:Opportunities"));
        opportunitiesPermission.AddChild(CrmAppPermissions.Opportunities.Create, L("Permission:Opportunities.Create"));
        opportunitiesPermission.AddChild(CrmAppPermissions.Opportunities.Edit, L("Permission:Opportunities.Edit"));
        opportunitiesPermission.AddChild(CrmAppPermissions.Opportunities.Delete, L("Permission:Opportunities.Delete"));
        var leadsPermission = crmAppGroup.AddPermission(CrmAppPermissions.Leads.Default, L("Permission:Leads"));
        leadsPermission.AddChild(CrmAppPermissions.Leads.Create, L("Permission:Leads.Create"));
        leadsPermission.AddChild(CrmAppPermissions.Leads.Edit, L("Permission:Leads.Edit"));
        leadsPermission.AddChild(CrmAppPermissions.Leads.Delete, L("Permission:Leads.Delete"));
        var productsPermission = crmAppGroup.AddPermission(CrmAppPermissions.Products.Default, L("Permission:Products"));
        productsPermission.AddChild(CrmAppPermissions.Products.Create, L("Permission:Products.Create"));
        productsPermission.AddChild(CrmAppPermissions.Products.Edit, L("Permission:Products.Edit"));
        productsPermission.AddChild(CrmAppPermissions.Products.Delete, L("Permission:Products.Delete"));
        var servicesPermission = crmAppGroup.AddPermission(CrmAppPermissions.Services.Default, L("Permission:Services"));
        servicesPermission.AddChild(CrmAppPermissions.Services.Create, L("Permission:Services.Create"));
        servicesPermission.AddChild(CrmAppPermissions.Services.Edit, L("Permission:Services.Edit"));
        servicesPermission.AddChild(CrmAppPermissions.Services.Delete, L("Permission:Services.Delete"));
        var salesPermission = crmAppGroup.AddPermission(CrmAppPermissions.Sales.Default, L("Permission:Sales"));
        salesPermission.AddChild(CrmAppPermissions.Sales.Create, L("Permission:Sales.Create"));
        salesPermission.AddChild(CrmAppPermissions.Sales.Edit, L("Permission:Sales.Edit"));
        salesPermission.AddChild(CrmAppPermissions.Sales.Delete, L("Permission:Sales.Delete"));
        var vendorsPermission = crmAppGroup.AddPermission(CrmAppPermissions.Vendors.Default, L("Permission:Vendors"));
        vendorsPermission.AddChild(CrmAppPermissions.Vendors.Create, L("Permission:Vendors.Create"));
        vendorsPermission.AddChild(CrmAppPermissions.Vendors.Edit, L("Permission:Vendors.Edit"));
        vendorsPermission.AddChild(CrmAppPermissions.Vendors.Delete, L("Permission:Vendors.Delete"));
        var supportCasesPermission = crmAppGroup.AddPermission(CrmAppPermissions.SupportCases.Default, L("Permission:SupportCases"));
        supportCasesPermission.AddChild(CrmAppPermissions.SupportCases.Create, L("Permission:SupportCases.Create"));
        supportCasesPermission.AddChild(CrmAppPermissions.SupportCases.Edit, L("Permission:SupportCases.Edit"));
        supportCasesPermission.AddChild(CrmAppPermissions.SupportCases.Delete, L("Permission:SupportCases.Delete"));
        var todoTasksPermission = crmAppGroup.AddPermission(CrmAppPermissions.TodoTasks.Default, L("Permission:TodoTasks"));
        todoTasksPermission.AddChild(CrmAppPermissions.TodoTasks.Create, L("Permission:TodoTasks.Create"));
        todoTasksPermission.AddChild(CrmAppPermissions.TodoTasks.Edit, L("Permission:TodoTasks.Edit"));
        todoTasksPermission.AddChild(CrmAppPermissions.TodoTasks.Delete, L("Permission:TodoTasks.Delete"));
        var rewardsPermission = crmAppGroup.AddPermission(CrmAppPermissions.Rewards.Default, L("Permission:Rewards"));
        rewardsPermission.AddChild(CrmAppPermissions.Rewards.Create, L("Permission:Rewards.Create"));
        rewardsPermission.AddChild(CrmAppPermissions.Rewards.Edit, L("Permission:Rewards.Edit"));
        rewardsPermission.AddChild(CrmAppPermissions.Rewards.Delete, L("Permission:Rewards.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CrmAppResource>(name);
    }
}
