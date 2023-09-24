using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Mediator.Authentication;
using RealEstate.Mediator.MapperProfile;
using System.Reflection;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;

IConfigurationRoot config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("JWT",
       new OpenApiSecurityScheme
       {
           Description = "JWT Authorization header using the Bearer scheme.",
           Name = "Authorization",
           In = ParameterLocation.Header,
           Type = SecuritySchemeType.Http,
           Scheme = "bearer"
       });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Real Estate API", Version = "v1" });
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
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
    byte[] key = Encoding.UTF8.GetBytes(config.GetSection("Security:Tokens:Key").Value.ToString());
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

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();

}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (IServiceScope scope = app.Services.CreateScope())
{
    UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        _ = await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("Standard"))
    {
        _ = await roleManager.CreateAsync(new IdentityRole("Standard"));
    }

    if (await userManager.FindByNameAsync("admin") == null)
    {
        IdentityUser adminUser = new()
        {
            UserName = "admin",
            Email = "admin@example.com"
        };

        IdentityResult result = await userManager.CreateAsync(adminUser, "AdminPassword123!");

        _ = result.Succeeded ? await userManager.AddToRoleAsync(adminUser, "Admin") : throw new Exception("Failed to create admin user");
    }


    if (await userManager.FindByNameAsync("user") == null)
    {
        IdentityUser standardUser = new()
        {
            UserName = "user",
            Email = "user@example.com"
        };

        IdentityResult result = await userManager.CreateAsync(standardUser, "UserPassword123!");

        _ = result.Succeeded
            ? await userManager.AddToRoleAsync(standardUser, "Standard")
            : throw new Exception("Failed to create standard user");
    }
}

app.Run();
