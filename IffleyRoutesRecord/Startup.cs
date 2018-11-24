using IffleyRoutesRecord.DatabaseOptions;
using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.ExistingData;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Logic.Validators;
using IffleyRoutesRecord.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace IffleyRoutesRecord
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

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

            services.AddTransient<IProblemRequestValidator, ProblemRequestValidator>();

            PopulateDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Iffley/swagger.json", "Iffley API");
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseResponseCaching().UseMvc();
        }

        private static void RegisterManagers(IServiceCollection services)
        {
            services.AddTransient<IStyleSymbolManager, StyleSymbolManager>();
            services.AddTransient<IRuleManager, RuleManager>();
            services.AddTransient<IHoldManager, HoldManager>();
            services.AddTransient<IGradeManager, GradeManager>();
            services.AddTransient<IProblemReader, ProblemReader>();
            services.AddTransient<IProblemCreator, ProblemCreator>();
        }

        private void SetupDatabase(IServiceCollection services)
        {
            string databaseType = Configuration["Database:DatabaseType"];
            string existingDataFilePath = Configuration["Database:ExistingDataPath"];

            if (databaseType == DatabaseType.Real.ToString())
            {
                services.AddDbContext<IffleyRoutesRecordContext>(
                    options => options.UseSqlite(Configuration["Database:ConnectionString"]));
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
                    context, existingDataFilePath, serviceProvider.GetRequiredService<IProblemRequestValidator>());
                existingProblemsPopulater.Populate(validate);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
