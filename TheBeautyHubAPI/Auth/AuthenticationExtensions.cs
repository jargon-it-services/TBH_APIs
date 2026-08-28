using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TheBeautyHubAPI.Auth
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddBeautyHubAuth(this IServiceCollection services, IConfiguration configuration)
        {
            ApplyEnvironmentOverrides(configuration);

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<AuthCenterOptions>(configuration.GetSection(AuthCenterOptions.SectionName));

            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings are missing. They must match AuthCenter.");

            if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || jwtSettings.SecretKey.Length < 32)
                throw new InvalidOperationException("JwtSettings:SecretKey must be at least 32 characters and match AuthCenter.");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            {
                KeyId = AuthCenterClaimTypes.SigningKeyId
            };

            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddHttpClient<IAuthCenterTokenValidator, AuthCenterTokenValidator>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(8);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = signingKey,
                    ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                    TryAllIssuerSigningKeys = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    IssuerSigningKeyResolver = (_, _, _, _) => new[] { signingKey }
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = JwtTokenValidation.OnMessageReceived,
                    OnAuthenticationFailed = JwtTokenValidation.OnAuthenticationFailed,
                    OnTokenValidated = JwtTokenValidation.OnTokenValidated,
                    OnChallenge = JwtTokenValidation.OnChallenge,
                    OnForbidden = JwtTokenValidation.OnForbidden
                };
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        public static void AddBeautyHubSwaggerSecurity(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Paste the AuthCenter login token (data.token only). Swagger adds the Bearer prefix."
            });

            options.OperationFilter<AllowAnonymousOperationFilter>();
        }

        public static void ApplyEnvironmentOverrides(IConfiguration configuration)
        {
            SetIfPresent(configuration, "JWT_SECRET", "JwtSettings:SecretKey");
            SetIfPresent(configuration, "JWT_ISSUER", "JwtSettings:Issuer");
            SetIfPresent(configuration, "JWT_AUDIENCE", "JwtSettings:Audience");
            SetIfPresent(configuration, "AUTHCENTER_BASE_URL", "AuthCenter:BaseUrl");
            SetIfPresent(configuration, "AUTHCENTER_APPLICATION_NAME", "AuthCenter:ApplicationName");
        }

        private static void SetIfPresent(IConfiguration configuration, string envName, string configKey)
        {
            var value = Environment.GetEnvironmentVariable(envName);
            if (!string.IsNullOrWhiteSpace(value))
                configuration[configKey] = value;
        }
    }
}
