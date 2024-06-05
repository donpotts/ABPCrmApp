using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using CrmApp.Customers;
using CrmApp.Addresses;
using CrmApp.ProductCategories;
using CrmApp.ServiceCategories;
using CrmApp.Contacts;
using CrmApp.Opportunities;
using CrmApp.Leads;
using CrmApp.Products;
using CrmApp.Services;
using CrmApp.Sales;
using CrmApp.Vendors;
using CrmApp.SupportCases;
using CrmApp.TodoTasks;
using CrmApp.Rewards;

namespace CrmApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class CrmAppDbContext :
    AbpDbContext<CrmAppDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<SupportCase> SupportCases { get; set; }
    public DbSet<TodoTask> TodoTasks { get; set; }
    public DbSet<Reward> Rewards { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public CrmAppDbContext(DbContextOptions<CrmAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Customer>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Customers",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Address>().WithOne().HasForeignKey<Customer>(x => x.AddressId);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Contact>().WithMany().HasForeignKey(x => x.ContactId);
        });

        builder.Entity<Address>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Addresses",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<ProductCategory>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "ProductCategories",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<ServiceCategory>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "ServiceCategories",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Contact>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Contacts",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Address>().WithOne().HasForeignKey<Contact>(x => x.AddressId);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Reward>().WithMany().HasForeignKey(x => x.RewardId);
        });

        builder.Entity<Opportunity>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Opportunities",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Lead>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Leads",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Address>().WithOne().HasForeignKey<Lead>(x => x.AddressId);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Opportunity>().WithOne().HasForeignKey<Lead>(x => x.OpportunityId);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Contact>().WithOne().HasForeignKey<Lead>(x => x.ContactId);
        });

        builder.Entity<Product>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Products",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<ProductCategory>().WithMany().HasForeignKey(x => x.ProductCategoryId);
        });

        builder.Entity<Service>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Services",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<ServiceCategory>().WithMany().HasForeignKey(x => x.ServiceCategoryId);
        });

        builder.Entity<Sale>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Sales",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Vendor>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Vendors",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Address>().WithOne().HasForeignKey<Vendor>(x => x.AddressId);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Service>().WithMany().HasForeignKey(x => x.ServiceId);

            // ADD THE MAPPING FOR THE RELATION
            b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        });

        builder.Entity<SupportCase>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "SupportCases",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<TodoTask>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "TodoTasks",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Reward>(b =>
        {
            b.ToTable(CrmAppConsts.DbTablePrefix + "Rewards",
                CrmAppConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}
