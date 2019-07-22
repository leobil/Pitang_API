using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pitang.Api2.Context;
using Pitang.Api2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pitang.Api2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

namespace Pitang.Api2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddTransient<UsuarioService>();

            var sigingConfigurations = new SigingConfigurations();
            services.AddSingleton(sigingConfigurations);

            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                    Configuration.GetSection("TokenConfigurations")
                ).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(ao =>
            {
                ao.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ao.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bo =>
            {
                var paramsValidation = bo.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = sigingConfigurations._key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(a => {
                a.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });


            services.AddDbContext<appDbContext>(option => option.UseInMemoryDatabase("Pitang"));
            services.AddScoped<Usuario>();
            services.AddScoped<Phone>();

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UsuarioService toUsuarioServive)
        {
            List<Phone> loLstPhones = new List<Phone>();
            loLstPhones.Add(new Phone { area_code = 21, country_code = "55", number = 12313 });
            toUsuarioServive.UsuarioAdd(new Usuario()
            {
                firstName = "Leonardo",
                lastName = "Moura",
                email = "contato@leomoura.eti.br",
                password = "123456",
                phones = loLstPhones
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
