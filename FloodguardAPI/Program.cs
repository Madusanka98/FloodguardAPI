using AutoMapper;
using LearnAPI.Container;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using LearnAPI.HangfireJob;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<IRiverStationService, RiverStationService>();
builder.Services.AddTransient<IRiverService, RiverService>();
builder.Services.AddTransient<IHistoryDataService, HistoryDataService>();
builder.Services.AddTransient<IRefreshHandler, RefreshHandler>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRoleServicecs, UserRoleService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddDbContext<LearndataContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("apicon")));

// Add Hangfire services
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("apicon"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    }));

// Add the Hangfire server as IHostedService
builder.Services.AddHangfireServer();

var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var automapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCors(p => p.AddPolicy("corspolicy1", build =>
{
    build.WithOrigins("https://localhost:7249", "http://localhost:56281", "https://localhost:7143", "https://floodguard-api.azurewebsites.net").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCors(p => p.AddDefaultPolicy(build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
{
    options.Window = TimeSpan.FromSeconds(10);
    options.PermitLimit = 1;
    options.QueueLimit = 0;
    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode = 401);

string logpath = builder.Configuration.GetSection("Logging:Logpath").Value;
var _logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logpath)
    .CreateLogger();
builder.Logging.AddSerilog(_logger);

var _jwtsetting = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(_jwtsetting);

var app = builder.Build();

app.MapGet("/minimalapi", () => "Nihira Techiees");

app.MapGet("/getchannel", (string channelname) => "Welcome to " + channelname).WithOpenApi(opt =>
{
    var parameter = opt.Parameters[0];
    parameter.Description = "Enter Channel Name";
    return opt;
});


app.UseRateLimiter();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseCors("corspolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseRouting();

// Configure Hangfire Dashboard with basic authentication
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireCustomBasicAuthenticationFilter
    {
        User = builder.Configuration["HangfireSettings:User"],
        Pass = builder.Configuration["HangfireSettings:Pass"]
    }}
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Register the recurring job

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();
    var riverStations = serviceProvider.GetRequiredService<IRiverStationService>();
    var historyDataService = serviceProvider.GetRequiredService<IHistoryDataService>();

    var sampleJob = new SampleJob(riverStations, historyDataService);

    // Define the Sri Lanka time zone
    var sriLankaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Colombo");

    // Register the job with the specified time zone
    recurringJobManager.AddOrUpdate(
        "Save Prediction Data",
        () => sampleJob.Execute(),
        "05 9 * * *", // Triggers at 1:10 AM daily in Sri Lanka time
        sriLankaTimeZone);
}

app.Run();
