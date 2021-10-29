using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Modul6Hw2.Data;
using Modul6Hw2.Data.Abstractions;
using Modul6Hw2.Data.Entities;
using Modul6Hw2.Data.Repositories;
using Modul6HW2.Configuration;
using Modul6HW2.Mapper;
using Modul6HW2.Services.Abstractions;
using Modul6HW2.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6HW2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["SqlConnection"]));

            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));*/

            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            services.Configure<JwtBearerTokenSettings>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Modul6HW2", Version = "v1" });
            });

            services.AddTransient<ITokensRepositori, TokensRepositori>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokensRepositori, TokensRepositori>();
            services.AddTransient<IUserRepositori, UserRepositori>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modul6HW2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
