using Foosball.Hubs;
using Foosball.Logic;
using Foosball.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Repository;
using Swashbuckle.AspNetCore.Swagger;

namespace Foosball
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
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGridSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;

            services.AddScoped<ClaimRequirementFilter>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            //Repository
            services.AddScoped<ILeaderboardViewRepository, LeaderboardViewRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IMatchupResultRepository, MatchupResultRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLoginInfoRepository, UserLoginInfoRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();

            //Logic
            services.AddScoped<IAchievementsService, AchievementsService>();
            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<IMatchupHistoryCreator, MatchupHistoryCreator>();
            services.AddScoped<ISeasonLogic, SeasonLogic>();
            services.AddScoped<IAccountLogic, AccountLogic>();
            services.AddScoped<IRating, EloRating>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IEmailLogic, EmailLogic>();

            //Hubs
            services.AddSingleton<IActivitySensorHub, ActivitySensorHub>();
            services.AddSingleton<IMatchAddedHub, MatchAddedHub>();

            services.AddSignalR();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSignalR(routes =>
            {
                routes.MapHub<MatchAddedHub>("/matchAddedHub");
                routes.MapHub<ActivitySensorHub>("/activitySensorHub");
            });

            app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
