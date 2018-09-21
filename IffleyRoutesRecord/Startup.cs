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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
            //var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = ":memory:" };
            //string connectionString = connectionStringBuilder.ToString();
            //var connection = new SqliteConnection(connectionString);
            //services
            //  .AddEntityFrameworkSqlite()
            //  .AddDbContext<IffleyRoutesRecordContext>(
            //    options => options.UseSqlite(connection)
            //  );

            services.AddDbContext<IffleyRoutesRecordContext>(
                options => options.UseSqlite(Configuration["Database:ConnectionString"]));

            var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<IffleyRoutesRecordContext>();

            //context.Database.OpenConnection();
            //context.Database.EnsureCreated();

            //context.TechGrade.Add(new Models.Entities.TechGrade()
            //{
            //    Name = "Test Tech Grade",
            //    Rank = 1
            //});
            //context.SaveChanges();

            var converter = new ConvertExistingDataToJson(context);
            converter.Convert();
        }
    }
}
