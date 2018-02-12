using System.Linq;
using FoosballCore.OldLogic;
using FoosballCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace FoosballCore
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
            services.AddIdentityWithMongoStores(Configuration.GetConnectionString("DefaultConnectionMongoDB"))
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            
            //Repository
            services.AddScoped<ILeaderboardViewRepository, LeaderboardViewRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IMatchupResultRepository, MatchupResultRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Logic
            services.AddScoped<IAchievementsService, AchievementsService>();
            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<IMatchupHistoryCreator, MatchupHistoryCreator>();
            services.AddScoped<ISeasonLogic, SeasonLogic>();
            services.AddScoped<IRating, EloRating>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
