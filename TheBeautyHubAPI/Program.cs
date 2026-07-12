using Microsoft.EntityFrameworkCore;
using TheBeautyHubData.Context;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubCore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework and SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BeautyHubDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFirmRepository, FirmRepository>();
builder.Services.AddScoped<IFirmDetailsRepository, FirmDetailsRepository>();
builder.Services.AddScoped<IPlansRepository, PlansRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IExpensesTypeRepository, ExpensesTypeRepository>();
builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
builder.Services.AddScoped<ITransactionRulesRepository, TransactionRulesRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportForAccountRepository, ReportForAccountRepository>();
builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();

// Register business services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFirmService, FirmService>();
builder.Services.AddScoped<IFirmDetailsService, FirmDetailsService>();
builder.Services.AddScoped<IPlansService, PlansService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IExpensesTypeService, ExpensesTypeService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<ITransactionRulesService, TransactionRulesService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionDetailService, TransactionDetailService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportForAccountService, ReportForAccountService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IExceptionLogService, ExceptionLogService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "The Beauty Hub API",
        Version = "v1",
        Description = "API for managing Beauty Hub accounts, users, firms, plans, subscriptions, wallets, expenses, services, transactions, reports, partners, sessions, and logs"
    });
});

// Configure CORS (optional - for frontend integration)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

// Configure the HTTP request pipeline
app.UseRouting();

// IMPORTANT: Disable HTTPS redirect in production on Render
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// IMPORTANT: Listen on all interfaces
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
