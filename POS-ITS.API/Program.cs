using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POS_ITS.API.AutoMapper;
using POS_ITS.API.Middlewares;
using POS_ITS.DATA;
using POS_ITS.REPOSITORIES.InventoryRepository;
using POS_ITS.REPOSITORIES.ProductRepository;
using POS_ITS.REPOSITORIES.SalesRepository;
using POS_ITS.REPOSITORIES.UserRepository;
using POS_ITS.SERVICE.InventoryService;
using POS_ITS.SERVICE.ProductService;
using POS_ITS.SERVICE.SalesService;
using POS_ITS.SERVICE.UserService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISalesService, SalesService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Logging.ClearProviders();
var log4netConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "log4net.config");
builder.Logging.AddLog4Net(log4netConfigPath);
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(t =>
//{
//    t.RequireHttpsMetadata = false;
//    t.SaveToken = true;
//    t.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//        ValidAudience = builder.Configuration["JwtSettings:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//    };
//    // Add event handlers for logging
//    t.Events = new JwtBearerEvents
//    {
//        OnAuthenticationFailed = context =>
//        {
//            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
//            Console.WriteLine($"{context.Request.Headers["Authorization"].ToString()}");
//            return Task.CompletedTask;
//        },
//        OnTokenValidated = context =>
//        {
//            Console.WriteLine("Token validated successfully");
//            return Task.CompletedTask;
//        },
//        OnMessageReceived = context =>
//        {
//            var authHeader = context.Request.Headers["Authorization"].ToString();
//            if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//            {
//                context.Token = authHeader.Substring("Bearer ".Length).Trim();
//                Console.WriteLine($"Token extracted: {context.Token}");
//            }
//            else
//            {
//                Console.WriteLine("No token found in the Authorization header.");
//            }
//            return Task.CompletedTask;
//        }
//    };
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("CashierPolicy", policy => policy.RequireRole("Cashier"));
//});

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    s.AddSecurityDefinition("AuthKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "AuthKey",
        Type = SecuritySchemeType.ApiKey,
        Description = "Custom Auth Key Header"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "AuthKey"
                }
            },
            new string[] {}
        }
    });

    //s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    Description = @"JWT Authorization header using the Bearer scheme. 
    //          Enter 'Bearer' [space] and then your token in the text input below.",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer"
    //});

    //s.AddSecurityRequirement(new OpenApiSecurityRequirement()
    //    {
    //        {
    //          new OpenApiSecurityScheme
    //          {
    //            Reference = new OpenApiReference
    //            {
    //              Type = ReferenceType.SecurityScheme,
    //              Id = "Bearer"
    //            },
    //            Scheme = "oauth2",
    //            Name = "Bearer",
    //            In = ParameterLocation.Header,
    //          },
    //          new List<string>()
    //        }
    //    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<AuthKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
