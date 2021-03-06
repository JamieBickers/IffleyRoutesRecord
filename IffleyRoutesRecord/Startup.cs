﻿using IffleyRoutesRecord.DatabaseOptions;
using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.ExistingData;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Logic.Validators;
using IffleyRoutesRecord.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using IffleyRoutesRecord.Auth;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authorization;

namespace IffleyRoutesRecord
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            SetupDatabase(services);
            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Iffley", new Info
                {
                    Title = "Iffley API",
                    Description = "The API for viewing and recording Iffley routes."
                });

                options.IncludeXmlComments("IffleyRoutesRecord.xml", true);
            });

            RegisterManagers(services);
            RegisterAuth(services);

            services.AddTransient<ProblemRequestValidator>();
            services.AddTransient<IssueRequestValidator>();

            PopulateDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Iffley/swagger.json", "Iffley API");
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseResponseCaching().UseMvc();
        }

        private static void RegisterManagers(IServiceCollection services)
        {
            services.AddTransient<IssueManager>();
            services.AddTransient<StyleSymbolManager>();
            services.AddTransient<RuleManager>();
            services.AddTransient<HoldManager>();
            services.AddTransient<IGradeManager, GradeManager>();
            services.AddTransient<ProblemReader>();
            services.AddTransient<ProblemCreator>();
        }

        private void SetupDatabase(IServiceCollection services)
        {
            string databaseType = Configuration["Database:DatabaseType"];
            string existingDataFilePath = Configuration["Database:ExistingDataPath"];

            if (databaseType == DatabaseType.Real.ToString())
            {
                services.AddDbContext<IffleyRoutesRecordContext>(options =>
                    options.UseNpgsql(Configuration["Database:ConnectionString"]));
            }
            else if (databaseType == DatabaseType.Memory.ToString())
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = ":memory:" };
                string connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);

                services.AddEntityFrameworkSqlite()
                    .AddDbContext<IffleyRoutesRecordContext>(
                    options => options.UseSqlite(connection));

                var serviceProvider = services.BuildServiceProvider();
                var context = serviceProvider.GetRequiredService<IffleyRoutesRecordContext>();

                context.Database.OpenConnection();
                context.Database.EnsureCreated();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void PopulateDatabase(IServiceCollection services)
        {
            string existingDataFilePath = Configuration["Database:ExistingDataPath"];
            string populateOption = Configuration["Database:PopulateOption"];
            bool validate = Configuration.GetValue<bool>("Database:ValidateData");

            if (populateOption == DatabasePrePopulationOptions.None.ToString())
            {
                // Do nothing
            }
            else if (populateOption == DatabasePrePopulationOptions.OnlyStaticData.ToString())
            {
                var serviceProvider = services.BuildServiceProvider();
                var context = serviceProvider.GetRequiredService<IffleyRoutesRecordContext>();

                var staticDataPopulater = new PopulateDatabaseWithStaticData(context, existingDataFilePath);
                staticDataPopulater.Populate();
            }
            else if (populateOption == DatabasePrePopulationOptions.StaticDataAndExistingProblems.ToString())
            {
                var serviceProvider = services.BuildServiceProvider();
                var context = serviceProvider.GetRequiredService<IffleyRoutesRecordContext>();

                var staticDataPopulater = new PopulateDatabaseWithStaticData(context, existingDataFilePath);
                staticDataPopulater.Populate();

                var existingProblemsPopulater = new PopulateDatabaseWithExistingProblems(
                    context, existingDataFilePath, serviceProvider.GetRequiredService<ProblemRequestValidator>());
                existingProblemsPopulater.Populate(validate);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void RegisterAuth(IServiceCollection services)
        {
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserRoles.Standard, policy => policy.Requirements.Add(new HasScopeRequirement(UserRoles.Standard, domain)));
                options.AddPolicy(UserRoles.Admin, policy => policy.Requirements.Add(new HasScopeRequirement(UserRoles.Admin, domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }
    }
}
