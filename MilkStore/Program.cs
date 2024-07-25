using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MilkStore_BAL.Mapper;
using MilkStore_BAL.Services.Implements;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;
using MilkStore_DAL.UnitOfWorks.Implements;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System.Text;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using MilkStore_BAL.BackgroundServices.Interfaces;
using MilkStore_BAL.BackgroundServices.Implements;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Connection string
builder.Services.AddDbContext<MomAndKidsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Set policy permission for roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireClaim("RoleId", "1"));
    options.AddPolicy("RequireStaffRole", policy => policy.RequireClaim("RoleId", "2"));
    options.AddPolicy("RequireCustomerRole", policy => policy.RequireClaim("RoleId", "3"));
    options.AddPolicy("RequireAdminOrStaffRole", policy => policy.RequireClaim("RoleId", "1", "2"));
    options.AddPolicy("RequireAdminOrCustomerRole", policy => policy.RequireClaim("RoleId", "1", "3"));
    options.AddPolicy("RequireStaffOrCustomerRole", policy => policy.RequireClaim("RoleId", "2", "3"));
    options.AddPolicy("RequireAllRoles", policy => policy.RequireClaim("RoleId", "1", "2", "3"));
});

//Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Auto mapper
builder.Services.AddAutoMapper(typeof(Program), typeof(MappingProfile));

// Register in-memory caching
builder.Services.AddMemoryCache();

// Service containers
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IVoucherOfShopService, VoucherOfShopService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Background service containers
builder.Services.AddScoped<IOrderBackgroundService, OrderBackgroundService>();
builder.Services.AddScoped<IProductBackgroundService, ProductBackgroundService>();

builder.Services.AddCors();

builder.Services.AddHttpContextAccessor();

// Configure Firebase
//FirebaseApp.Create(new AppOptions()
//{
//    Credential = GoogleCredential.FromFile(builder.Configuration["Firebase:Chat:CredentialPath"]),
//});

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    }));
builder.Services.AddHangfireServer();

// Configure Swagger
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

// Schedule the recurring job (Hangfire)
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate(
    "RejectExpiredOrder",
    () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IOrderBackgroundService>().RejectExpiredOrder(),
    Cron.MinuteInterval(2)
    );

recurringJobManager.AddOrUpdate(
    "RemoveHiddenProductInCustomerCarts",
    () => app.Services.CreateScope().ServiceProvider.GetRequiredService<IProductBackgroundService>().RemoveHiddenProductInCustomerCarts(),
    Cron.Minutely
    );

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
