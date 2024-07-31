using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Swashbuckle.AspNetCore.Filters;
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

builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("AuthKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "AuthKey",
        Type = SecuritySchemeType.ApiKey
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
    s.OperationFilter<SecurityRequirementsOperationFilter>();
});

//builder.Services.AddSwaggerGen(s =>
//{ 
//    s.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    s.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Authorization"
//                }
//            },
//            new string[] {}
//        }
//    });

//    s.OperationFilter<SecurityRequirementsOperationFilter>();
//});

builder.Services.AddAuthentication().AddJwtBearer(t => {
    t.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
    t.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                Console.WriteLine($"{context.Request.Headers["Authorization"].ToString()}");
                return Task.CompletedTask;
            }
    };
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
