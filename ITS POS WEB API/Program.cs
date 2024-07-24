using ITS_POS.Data;
using ITS_POS_WEB_API.Controllers;
using ITS_POS.Services;
using ITS_POS.Entities;
using ITS_POS_WEB_API.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add the custom AuthKey header
    c.AddSecurityDefinition("AuthKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "AuthKey",
        Type = SecuritySchemeType.ApiKey,
        Description = "Custom Auth Key Header"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

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

app.UseMiddleware<AuthKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = new DataContextDb();
    ServiceBase.Initialize(context);
};

app.Run();
