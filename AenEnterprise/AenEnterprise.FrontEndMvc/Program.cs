using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DataAccess;
using AenEnterprise.ServiceImplementations.Implementation;
using AenEnterprise.ServiceImplementations.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Builder;
using AenEnterprise.FrontEndMvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using AenEnterprise.DomainModel.CookieStorage;
using Mapster;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using AenEnterprise.ServiceImplementations;
using AenEnterprise.ServiceImplementations.Mapping.Automappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Start EF connectionstring
builder.Services.AddDbContext<AenEnterpriseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AenDbEnterpriseConnection")),ServiceLifetime.Scoped);
//End
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddTransient<IInvoiceItemRepository, InvoiceItemRepository>();
builder.Services.AddTransient<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddTransient<ISalesOrderRepository, SalesOrderRepository>();
//builder.Services.AddTransient<ISalesOrderService, SalesOrderService>();
builder.Services.AddTransient<ISalesOrderService, SalesService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ISalesOrderStatusRepository, SalesOrderStatusRepository>();
builder.Services.AddTransient<IUnitRepository, UnitRepository>();
builder.Services.AddTransient<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IDeliveryOrderRepository, DeliveryOrderRepository>();
builder.Services.AddTransient<IDeliveryOrderItemRepository, DeliveryOrderItemRepository>();
builder.Services.AddTransient<IDispatcheRepository, DispatcheOrderRepository>();
builder.Services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddTransient<IPurchaseItemRepository, PurchaseItemRepository>();

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICookieImplementation, CookieImplementation>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();

// Configure session options here if needed
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});

//Automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

 
//Jwt Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddHttpContextAccessor();
//Enable CORS
//app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

//void configureMapster()
//{
//    TypeAdapterConfig<DeliveryOrder, DeliveryOrderView>.NewConfig()
//             .Map(dest => dest.CustomerName, src => src.SalesOrder.Customer.Name)
//             .Map(dest => dest.DeliveryOrderItems, src => src.DeliveryOrderItem.Adapt<List<DeliveryOrderItemView>>());
//}

var app = builder.Build();

//Report Environment configure
var env = app.Services.GetRequiredService<IHostEnvironment>();
if(env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
 

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//configureMapster();