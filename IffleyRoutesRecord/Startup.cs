using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.ExistingData;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Logic.Validators;
using IffleyRoutesRecord.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddDbContext<IffleyRoutesRecordContext>(
                options => options.UseSqlite(Configuration["Database:ConnectionString"]));
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
            services.AddTransient<PopulateDatabaseWithExistingProblems, PopulateDatabaseWithExistingProblems>();

            var serviceProvider = services.BuildServiceProvider();
            var existingProblemsPopulater = serviceProvider.GetRequiredService<PopulateDatabaseWithExistingProblems>();
            existingProblemsPopulater.Populate();
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
    }
}
