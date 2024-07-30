using ITS_POS.Data;
using ITS_POS_WEB_API.Controllers;
using ITS_POS.Services;
using ITS_POS.Entities;
using ITS_POS_WEB_API.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration.Xml;
using System.Text;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//builder.Services.AddSwaggerGen(s =>
//{
//    s.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

//    s.AddSecurityDefinition("AuthKey", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "AuthKey",
//        Type = SecuritySchemeType.ApiKey,
//        Description = "Custom Auth Key Header"
//    });

//    s.AddSecurityRequirement(new OpenApiSecurityRequirement
//        {
//            {
//                new OpenApiSecurityScheme
//                {
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "AuthKey"
//                    }
//                },
//                new string[] {}
//            }
//        });
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(t => {
        t.RequireHttpsMetadata = false;
        t.SaveToken = true;
        t.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IProductManagementService, ProductManagementService>();
builder.Services.AddScoped<IInventoryManagementService, InventoryManagementService>();
builder.Services.AddScoped<ISalesTransactionService, SalesTransactionService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddDbContext<DataContextDb>(options => options.UseInMemoryDatabase("ITS-POS"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<AuthKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
