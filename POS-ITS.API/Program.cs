using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POS_ITS.API.AutoMapper;
using POS_ITS.API.Middlewares;
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

//builder.Services.AddDbContext<DataDbContext>();

if (builder.Environment.IsProduction())
{
    var keyVaultURI = builder.Configuration["KeyVaultSettings:KeyVaultURI"]!;
    var clientID = builder.Configuration["KeyVaultSettings:ClientID"]!;
    var clientSecret = builder.Configuration["KeyVaultSettings:ClientSecret"]!;
    var directoryID = builder.Configuration["KeyVaultSettings:DirectoryID"]!;

    var credential = new ClientSecretCredential(directoryID, clientID, clientSecret);

    builder.Configuration.AddAzureKeyVault(keyVaultURI, clientID, clientSecret, new DefaultKeyVaultSecretManager());

    var client = new SecretClient(new Uri(keyVaultURI), credential);
    var endpointUri = client.GetSecret("ProdEndpointUri").Value.Value.ToString();
    var primaryKey = client.GetSecret("ProdPrimaryKey").Value.Value.ToString();
    var dbName = client.GetSecret("ProdDatabaseName").Value.Value.ToString();

    builder.Configuration["CosmosDbSettings:EndpointUri"] = endpointUri;
    builder.Configuration["CosmosDbSettings:PrimaryKey"] = primaryKey;
    builder.Configuration["CosmosDbSettings:DatabaseName"] = dbName;

    var jwtKey = client.GetSecret("ProdJWTKey").Value.Value.ToString();
    builder.Configuration["JwtSettings:Key"] = jwtKey;

    var azureClientId = client.GetSecret("AzureClientId").Value.Value.ToString();
    var azureTenantId = client.GetSecret("AzureTenantId").Value.Value.ToString();
    var azureAudience = client.GetSecret("AzureAudience").Value.Value.ToString();

    builder.Configuration["AzureAd:ClientId"] = azureClientId;
    builder.Configuration["AzureAd:TenantId"] = azureTenantId;
    builder.Configuration["AzureAd:Audience"] = azureAudience;
}

var cosmosClient = new CosmosClient(builder.Configuration["CosmosDbSettings:EndpointUri"]!, builder.Configuration["CosmosDbSettings:PrimaryKey"]!);
builder.Services.AddSingleton(cosmosClient);
string databaseName = builder.Configuration["CosmosDbSettings:DatabaseName"]!;

// Register repositories
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRepository>(provider =>
{
    var client = provider.GetRequiredService<CosmosClient>();
    return new UserCosmosRepository(client, databaseName);
});

//builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductRepository>(provider =>
{
    var client = provider.GetRequiredService<CosmosClient>();
    return new ProductCosmosRepository(client, databaseName);
});

//builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryRepository>(provider =>
{
    var client = provider.GetRequiredService<CosmosClient>();
    return new InventoryCosmosRepository(client, databaseName);
});

//builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ISalesRepository>(provider =>
{
    var client = provider.GetRequiredService<CosmosClient>();
    return new SalesCosmosRepository(client, databaseName);
});

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(options =>
{
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidAudience = builder.Configuration["AzureAd:Audience"]!;
},
microsoftidentityoptions =>
{
    builder.Configuration.GetSection("AzureAd").Bind(microsoftidentityoptions);
},
"Bearer",
true);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("User.Admin"));

    options.AddPolicy("CashierPolicy", policy =>
        policy.RequireRole("User.Cashier"));
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

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
