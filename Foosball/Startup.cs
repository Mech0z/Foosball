﻿using Foosball.Hubs;
using Foosball.Logic;
using Foosball.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models;
using Repository;

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
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddScoped<ClaimRequirementFilter>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo{Title = "Foosball API", Version = "v1" });
            });

            //Repository
            services.AddScoped<ILeaderboardViewRepository, LeaderboardViewRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IMatchupResultRepository, MatchupResultRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLoginInfoRepository, UserLoginInfoRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IPlayerRankHistoryRepository, PlayerRankHistoryRepository>();
            
            //Logic
            services.AddScoped<IAchievementsService, AchievementsService>();
            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<IMatchupHistoryCreator, MatchupHistoryCreator>();
            services.AddScoped<ISeasonLogic, SeasonLogic>();
            services.AddScoped<IAccountLogic, AccountLogic>();
            services.AddScoped<IRating, EloRating>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IEmailLogic, EmailLogic>();
            services.AddScoped<IPlayerRankLogic, PlayerRankLogic>();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:4200","http://localhost:5000", "https://foosball.azurewebsites.net", "https://foosballapi.azurewebsites.net");
            }));

            services.AddSignalR();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/foosballHub");
                endpoints.MapControllers();
            });
        }
    }
}
