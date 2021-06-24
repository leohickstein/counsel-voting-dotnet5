using CounselVoting.Infrastructure.Context;
using CounselVoting.Infrastructure.Helper;
using CounselVoting.Infrastructure.Repository;
using CounselVoting.Infrastructure.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace CounselVoting.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CounselContext>(options =>
                options.UseSqlite(@"Data Source=CounselVoting.db"));

            //REPOSITORIES
            services.AddScoped<IMeasureRepository, MeasureRepository>();

            //SERVICES
            services.AddScoped<IMeasureService, MeasureService>();

            services.AddSingleton<IDateTimeService, DateTimeService>();

            var completionRuleStrategies = new Dictionary<string, ICompletionRule>();
            foreach (var type in AssemblyHelper.GetAllTypesThatImplementInterface<ICompletionRule>())
            {
                var instance = (ICompletionRule)Activator.CreateInstance(type);
                completionRuleStrategies.Add(type.Name, instance);
            }
            services.AddSingleton<IMeasureCompletionEvaluator>(new MeasureCompletionEvaluator(completionRuleStrategies));

            //HOSTED SERVICE
            services.AddHostedService<VotingEngineHostedService>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAny",
                                  builder =>
                                  {
                                      builder
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader()
                                          .AllowAnyMethod();
                                  });
            });

            services.AddControllers()
                .AddNewtonsoftJson(opts => {
                    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "CounselVoting.Api", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CounselContext dbContext)
        {
            if (env.IsDevelopment())
            {
                dbContext.Database.EnsureCreated();

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CounselVoting.Api v1"));
            }

            app.UseCors("AllowAny");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
