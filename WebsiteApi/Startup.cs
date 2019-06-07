using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigSettings;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Entities.DbModels;
using Services;

namespace WebsiteApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string MyAllowedSpecificOrigins = "_myAllowedSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Configure the DbContext
            services.AddDbContext<WebsiteApiDbContext>(options =>
                options.UseSqlServer(Configuration.GetSection("ConnectionStrings")["DefaultConnection"]));

            //Set up dependency injection
            services.AddTransient<IUserService<User>, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserRoleService, UserRoleService>();

            //Set up local CORS
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowedSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:4201", "http://localhost:4201")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });

            //Set up strongly-typed settings objects.
            var authSettingsSection = Configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(authSettingsSection);

            //Set up JWT authentication
            var authSettings = authSettingsSection.Get<AuthSettings>();
            var key = Encoding.ASCII.GetBytes(authSettings.SecretKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Create Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanViewApplicationList", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var user = context.User;
                        return user.IsInRole("Employee") || user.IsInRole("Manager");
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowedSpecificOrigins);
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
