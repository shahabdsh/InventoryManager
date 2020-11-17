using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using InventoryManager.Api.Models;
using InventoryManager.Api.Services;
using InventoryManager.Api.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InventoryManager.Api
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            services.Configure<InventoryDatabaseSettings>(
                Configuration.GetSection(nameof(InventoryDatabaseSettings)));

            services.AddSingleton<IItemService, ItemService>();
            services.AddSingleton<IItemSchemaService, ItemSchemaService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddTransient<IValidator<Item>, ItemValidator>();
            services.AddTransient<IValidator<ItemSchema>, ItemSchemaValidator>();

            BsonClassMaps.Map();

            services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication(x =>
                {
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = Configuration["Authentication:Jwt:Secret"];

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = UserService.GenerateTokenValidationParameters(key);

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetService<IUserService>();

                            var token = (context.SecurityToken as JwtSecurityToken)?.RawData;

                            if (string.IsNullOrEmpty(token) || userService.IsTokenRevoked(token))
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Todo: Set up HTTPS certificates in docker so you can remove this if
            if (!env.IsStaging())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
