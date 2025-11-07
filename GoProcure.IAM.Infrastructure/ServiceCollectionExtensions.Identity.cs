using GoProcure.IAM.Infrastructure.Auth;
using GoProcure.IAM.Infrastructure.Identity;
using GoProcure.IAM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.IAM.Infrastructure
{
    public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        // DbContext for Identity (same connection as app or a separate one)
        services.AddDbContext<IdentityDb>(opt =>
            opt.UseSqlServer(cfg.GetConnectionString("Default")));

        // Identity Core
        services
            .AddIdentityCore<AppUser>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 8;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<IdentityDb>()
            .AddSignInManager();

        // JWT
        var jwtSection = cfg.GetSection("Jwt");
        services.Configure<JwtOptions>(jwtSection);
        var jwt = jwtSection.Get<JwtOptions>()!;

        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false; // set true in prod behind TLS
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            })
            // Google OAuth
            .AddGoogle(google =>
            {
                google.ClientId = cfg["Authentication:Google:ClientId"]!;
                google.ClientSecret = cfg["Authentication:Google:ClientSecret"]!;
                // google.CallbackPath = "/signin-google"; // default
            })
            // Facebook OAuth
            .AddFacebook(fb =>
            {
                fb.AppId = cfg["Authentication:Facebook:AppId"]!;
                fb.AppSecret = cfg["Authentication:Facebook:AppSecret"]!;
                // fb.CallbackPath = "/signin-facebook"; // default
            });

        services.AddAuthorization();

        // Token service
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
}
