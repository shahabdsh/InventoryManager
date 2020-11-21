using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using InventoryManager.Api.Models;
using InventoryManager.Api.Options;
using InventoryManager.Api.Services;
using InventoryManager.Api.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace InventoryManager.Api
{
    public class Startup
    {
        private readonly string _devAndStagingCorsPolicy = nameof(_devAndStagingCorsPolicy);

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
                options.AddPolicy(name: _devAndStagingCorsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            services.Configure<InventoryDatabaseOptions>(
                Configuration.GetSection(nameof(InventoryDatabaseOptions)));

            services.Configure<AuthOptions>(
                Configuration.GetSection(nameof(AuthOptions)));

            services.AddHttpContextAccessor();

            services.AddScoped(provider =>
            {
                var accessor = provider.GetService<IHttpContextAccessor>();

                var claim = accessor.HttpContext.User.Claims.SingleOrDefault(x => x.Type == nameof(User.Id));

                return new UserContext()
                {
                    UserId = claim?.Value
                };
            });

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemSchemaService, ItemSchemaService>();
            services.AddScoped<IUserService, UserService>();

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
                    var key = Environment.GetEnvironmentVariable(EnvironmentVariableNames.JwtSecret);

                    if (string.IsNullOrEmpty(key))
                        throw new ArgumentException("Jwt secret is not initialized");

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

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseCors(_devAndStagingCorsPolicy);
            }
            else
            {
                app.UseCors();
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
