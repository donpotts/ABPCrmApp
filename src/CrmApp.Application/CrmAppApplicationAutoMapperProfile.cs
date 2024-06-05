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
using AutoMapper;

namespace CrmApp;

public class CrmAppApplicationAutoMapperProfile : Profile
{
    public CrmAppApplicationAutoMapperProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();
        CreateMap<Address, AddressDto>();
        CreateMap<CreateUpdateAddressDto, Address>();
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();
        CreateMap<ServiceCategory, ServiceCategoryDto>();
        CreateMap<CreateUpdateServiceCategoryDto, ServiceCategory>();
        CreateMap<Contact, ContactDto>();
        CreateMap<CreateUpdateContactDto, Contact>();
        CreateMap<Opportunity, OpportunityDto>();
        CreateMap<CreateUpdateOpportunityDto, Opportunity>();
        CreateMap<Lead, LeadDto>();
        CreateMap<CreateUpdateLeadDto, Lead>();
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<Service, ServiceDto>();
        CreateMap<CreateUpdateServiceDto, Service>();
        CreateMap<Sale, SaleDto>();
        CreateMap<CreateUpdateSaleDto, Sale>();
        CreateMap<Vendor, VendorDto>();
        CreateMap<CreateUpdateVendorDto, Vendor>();
        CreateMap<SupportCase, SupportCaseDto>();
        CreateMap<CreateUpdateSupportCaseDto, SupportCase>();
        CreateMap<TodoTask, TodoTaskDto>();
        CreateMap<CreateUpdateTodoTaskDto, TodoTask>();
        CreateMap<Reward, RewardDto>();
        CreateMap<CreateUpdateRewardDto, Reward>();
        CreateMap<Address, CrmApp.Customers.AddressLookupDto>();
        CreateMap<Address, CrmApp.Contacts.AddressLookupDto>();
        CreateMap<Address, CrmApp.Leads.AddressLookupDto>();
        CreateMap<Opportunity, CrmApp.Leads.OpportunityLookupDto>();
        CreateMap<Address, CrmApp.Vendors.AddressLookupDto>();
        CreateMap<ProductCategory, CrmApp.Products.ProductCategoryLookupDto>();
        CreateMap<ServiceCategory, CrmApp.Services.ServiceCategoryLookupDto>();
        CreateMap<Contact, CrmApp.Customers.ContactLookupDto>();
        CreateMap<Service, CrmApp.Vendors.ServiceLookupDto>();
        CreateMap<Product, CrmApp.Vendors.ProductLookupDto>();
        CreateMap<Reward, CrmApp.Contacts.RewardLookupDto>();
        CreateMap<Contact, CrmApp.Leads.ContactLookupDto>();
    }
}
