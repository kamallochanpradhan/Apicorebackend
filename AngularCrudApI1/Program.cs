using AngularCrudApI1;
using AngularCrudApI1.Repository;
using AngularCrudApI1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Serilog;
//E:\CGI\TESTINGPURPOSE\MyAspCoreAppln\AngularCrudApI1\Logs\

var builder = WebApplication.CreateBuilder(args);
#region All Serilog Configurationhere

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/MyAppllog.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

#endregion
var configuration = builder.Configuration;

// Add services to the container.
//just checking


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTRefreshTokens", Version = "v1" });

    /*This configuration is saying we are telling swagger we want to use 
     * authorization based on this defination*/
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "This site uses Bearrer token and you have to pass" +
        "Its a Bearer<<space>>Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    }); 

   /* This configuration is saying here swagger will use the token passed from UI and it 
    * will give the token to controller*/
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {{
        new OpenApiSecurityScheme
        {
            Reference=new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer",
            },
            Scheme="oauth2",
            Name="Bearer",
            In=ParameterLocation.Header
        },
        new List<string>()
    }
    });
});

var jwtKey = configuration["JwtSettings:Key"];
var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

TokenValidationParameters tokenValidation = new TokenValidationParameters
{
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    ValidateLifetime = true,

    //if you use any delegated permission then you can ,,,but i dont have so false
    ValidateAudience = false,

    /*if you use any azure kind of environment ,where same token
    //can use for multiple tennant people*/
    ValidateIssuer = false,

    /*if by default token expire jwt toke give 5min grace period...but we dont want
     * so zero*/
    ClockSkew= TimeSpan.Zero,
};

builder.Services.AddSingleton(tokenValidation);

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
{
    //custom validation done doing Jwt events
    //here we have passed the tokenValidation parameter
    jwtOptions.TokenValidationParameters = tokenValidation;
    jwtOptions.Events = new JwtBearerEvents();
    jwtOptions.Events.OnTokenValidated = async (context) =>
      {
          var ipAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
          var jwtService = context.Request.HttpContext.RequestServices.GetService<IjwtService>();
          var jwtToken = context.SecurityToken as JwtSecurityToken;

          if (!await jwtService.IsTokenValid(jwtToken.RawData, ipAddress))
              context.Fail("Invalid Token Details");
      };

});
   



//Dependencies mapping of service and interface here
builder.Services.AddTransient<IjwtService, JwtService>();


builder.Services.AddScoped<IStudenttRepository, StudenttRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyAngularDataContext>(x => x.UseSqlServer(connectionString));

//Enable CORS
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
      .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlerMiddleWare>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
