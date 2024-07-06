using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN231.API.Constants;
using PRN231.Repo.Implements;
using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;

namespace PRN231.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<MyDBContext>(options => options.UseSqlServer(GetConnectionString()));
        return services;
    }

    private static string GetConnectionString()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables(JwtConstant.JwtEnvironment)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:MyStoreDB"];

        return strConn;
    }

    public static IServiceCollection AddJwtValidation(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(JwtConstant.JwtEnvironment)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            Console.WriteLine($"Key: {de.Key}, Value: {de.Value}");

        var secretKey = configuration["JwtConstant:" + JwtConstant.SecretKey];
        var issuer = configuration["JwtConstant:" + JwtConstant.Issuer];

        Console.WriteLine($"SecretKey: {secretKey}");
        Console.WriteLine($"Issuer: {issuer}");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        return services;
    }
}