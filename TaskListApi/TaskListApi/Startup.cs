using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TaskListApi.Data;

namespace TaskListApi
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
            services.AddControllers().AddNewtonsoftJson(s => s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            string server = Configuration["DBServer"] ?? "ms-sql-server";
            string port = Configuration["DBPort"] ?? "1433";
            string user = Configuration["DBUser"] ?? "SA";
            string password = Configuration["DBPassword"] ?? "P3dr0@123";
            string database = Configuration["Database"] ?? "TaskListDb";

            services.AddDbContext<AppDbContext>(o => o.UseSqlServer($"Server={server},{port};Database={database};User ID={user};Password={password}"));
            services.AddSwaggerGen(c => {
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddScoped<ITagsRepo, TagsRepo>();
            services.AddScoped<ITasksRepo, TasksRepo>();
            services.AddScoped<ITaskListsRepo, TaskListsRepo>();
            services.AddScoped<ITaskTagsRepo, TaskTagsRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskListApi V1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDB.Population(app);
        }
    }
}
