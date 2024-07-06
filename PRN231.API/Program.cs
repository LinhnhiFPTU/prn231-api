using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PRN231.API.Extensions;
using PRN231.API.Middlewares;
using PRN231.API.Payload.Response;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "One or more validation errors occurred",
                Data = new
                {
                    errors
                }
            };

            return new JsonResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
        options.JsonSerializerOptions.WriteIndented = true;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "PRN231 API",
            Description = "Document for PRN231 API"
        }
    );
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("allow origin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddDatabase();
builder.Services.AddUnitOfWork();
builder.Services.AddJwtValidation();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();