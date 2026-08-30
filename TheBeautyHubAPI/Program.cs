using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using TheBeautyHubAPI.Auth;
using TheBeautyHubAPI.Helpers;
using TheBeautyHubData.Context;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubCore.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigureListenUrls(builder);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework and PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BeautyHubDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IFirmRepository, FirmRepository>();
//builder.Services.AddScoped<IFirmDetailsRepository, FirmDetailsRepository>();
//builder.Services.AddScoped<IPlansRepository, PlansRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IExpensesTypeRepository, ExpensesTypeRepository>();
builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
//builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
//builder.Services.AddScoped<ITransactionRulesRepository, TransactionRulesRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
//builder.Services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
//builder.Services.AddScoped<IReportRepository, ReportRepository>();
//builder.Services.AddScoped<IReportForAccountRepository, ReportForAccountRepository>();
//builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();

// Register business services
builder.Services.AddScoped<IFirmService, FirmService>();
//builder.Services.AddScoped<IFirmDetailsService, FirmDetailsService>();
//builder.Services.AddScoped<IPlansService, PlansService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IExpensesTypeService, ExpensesTypeService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
//builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
//builder.Services.AddScoped<ITransactionRulesService, TransactionRulesService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
//builder.Services.AddScoped<ITransactionDetailService, TransactionDetailService>();
//builder.Services.AddScoped<IReportService, ReportService>();
//builder.Services.AddScoped<IReportForAccountService, ReportForAccountService>();
//builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IExceptionLogService, ExceptionLogService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<ISalaryRuleService, SalaryRuleService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddScoped<TheBeautyHubAPI.Helpers.BranchLogoStorage>();
builder.Services.AddScoped<TheBeautyHubAPI.Helpers.StaffFileStorage>();
builder.Services.AddScoped<TheBeautyHubAPI.Helpers.ServicePhotoStorage>();
builder.Services.AddBeautyHubAuth(builder.Configuration);

// Configure AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    // GHSA-rvv3-g6hj-g44x: 12.0.1 has no patched MIT release; cap recursion so nested graphs cannot stack-overflow.
    cfg.Internal().ForAllMaps((_, mapping) => mapping.MaxDepth(32));
}, typeof(Program));

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "The Beauty Hub API",
        Version = "v1",
        Description = "API for The Beauty Hub. Protected endpoints require an AuthCenter Bearer token. Account identity comes from the token accountId claim."
    });
    c.AddBeautyHubSwaggerSecurity();
    c.OperationFilter<BranchSaveSwaggerFilter>();
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

var app = builder.Build();

// ✅ APPLY PENDING EF CORE MIGRATIONS
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BeautyHubDbContext>();
    try
    {
        // Apply pending migrations
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
        if (!app.Environment.IsDevelopment())
            throw;
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger")).AllowAnonymous();
app.MapControllers();

app.Run();

static void ConfigureListenUrls(WebApplicationBuilder builder)
{
    // Never call app.Run(url): Kestrel addresses are read-only after the host is built.
    // ASPNETCORE_URLS (Docker/Kestrel) wins. Otherwise PORT (PaaS). Production defaults to all interfaces.
    var aspNetUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
    if (!string.IsNullOrWhiteSpace(aspNetUrls))
        return;

    var port = Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrWhiteSpace(port))
    {
        builder.WebHost.UseUrls($"http://0.0.0.0:{port.Trim()}");
        return;
    }

    if (!builder.Environment.IsDevelopment())
        builder.WebHost.UseUrls("http://0.0.0.0:5000");
}