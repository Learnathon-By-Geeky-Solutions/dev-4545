using Employee.API;

using Employee.API.ExceptionHandler;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;

using System.Text;





var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.Configure<Employee.Infrastructure.Setttings.JwtSettings>(
    builder.Configuration.GetSection("Jwt"));


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(

     options =>

     {

         options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme

         {

             Description = "Standard Authorization Header using the Bearer Scheme (\"bearer {token}\")",

             In = ParameterLocation.Header,

             Name = "Authorization",

             Type = SecuritySchemeType.ApiKey

         });



         options.OperationFilter<SecurityRequirementsOperationFilter>(); //Matt Frear

     }

);



builder.Services.AddExceptionHandler<GlobalExceptionHandler>();



builder.Services.AddAppDI(builder.Configuration);

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtKey))

{

    throw new InvalidOperationException("JWT secret key is not configured.");

}



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>

    {

        options.TokenValidationParameters = new TokenValidationParameters

        {

            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))

        };

    });

var allowedOrigin = builder.Configuration["Cors:url"];

if (string.IsNullOrWhiteSpace(allowedOrigin))
{
    throw new InvalidOperationException("CORS origin is not configured. Please set 'Cors:url' in appsettings.json or environment variables.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});





var app = builder.Build();



// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())

{

    app.UseSwagger();

    app.UseSwaggerUI();

}



app.UseExceptionHandler(_ => { });



app.UseHttpsRedirection();

app.UseCors("AllowReactApp");


app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();



app.Run();