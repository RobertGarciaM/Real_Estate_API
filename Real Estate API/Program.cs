using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Real_Estate_API;
using RealEstate.Mediator.Authentication;
using RealEstate.Mediator.AutoMapperProfile;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(_ =>
{
    //_.EnableAnnotations();

    _.AddSecurityDefinition("JWT",
       new OpenApiSecurityScheme
       {
           Description = "JWT Authorization header using the Bearer scheme.",
           Name = "Authorization",
           In = ParameterLocation.Header,
           Type = SecuritySchemeType.Http,
           Scheme = "bearer"
       });

    _.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "JWT"
                        }
                    },
                    new List<string>()
                }
            });
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<InMemoryDbContext>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(config.GetSection("Security:Tokens:Key").Value.ToString());
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = config.GetSection("Security:Tokens:Issuer").Value.ToString(),
        ValidateAudience = true,
        ValidAudience = config.GetSection("Security:Tokens:Audience").Value.ToString(),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Security:Tokens:Key").Value.ToString())),
    };
});
builder.Services.AddDbContext<InMemoryDbContext>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("RealEstate.Mediator")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("Standard"))
    {
        await roleManager.CreateAsync(new IdentityRole("Standard"));
    }

    if (await userManager.FindByNameAsync("admin") == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = "admin",
            Email = "admin@example.com"
        };

        var result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            throw new Exception("Failed to create admin user");
        }
    }


    if (await userManager.FindByNameAsync("user") == null)
    {
        var standardUser = new IdentityUser
        {
            UserName = "user",
            Email = "user@example.com"
        };

        var result = await userManager.CreateAsync(standardUser, "UserPassword123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(standardUser, "Standard");
        }
        else
        {
            throw new Exception("Failed to create standard user");
        }
    }
}

app.Run();
