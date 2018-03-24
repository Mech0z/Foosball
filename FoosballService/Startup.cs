using System.Linq;
using FoosballCore.OldLogic;
using FoosballCore.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
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
            services.Configure<ConnectionStringsSettings>(Configuration.GetSection("ConnectionStrings"));
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
            services.AddScoped<IUserMappingRepository, UserMappingRepository>();
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();

            //Logic
            services.AddScoped<IAchievementsService, AchievementsService>();
            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<IMatchupHistoryCreator, MatchupHistoryCreator>();
            services.AddScoped<ISeasonLogic, SeasonLogic>();
            services.AddScoped<IRating, EloRating>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }

#if DEBUG
            TelemetryConfiguration.Active.DisableTelemetry = true;
#endif

            //app.UseCors("MyPolicy");

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
