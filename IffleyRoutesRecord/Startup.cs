using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<IffleyRoutesRecordContext>(
                options => options.UseSqlite(@"Data Source=C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\IffleyRoutes.db;"));

            services.AddTransient<IStyleSymbolManager, StyleSymbolManager>();
            services.AddTransient<IRuleManager, RuleManager>();
            services.AddTransient<IHoldManager, HoldManager>();
            services.AddTransient<IGradeManager, GradeManager>();
            services.AddTransient<IProblemReader, ProblemReader>();
            services.AddTransient<IProblemCreator, ProblemCreator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
