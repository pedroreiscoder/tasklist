using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public static class PrepDB
    {
        public static void Population(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        public static void SeedData(AppDbContext context)
        {
            Console.WriteLine("Applying migrations...");

            context.Database.Migrate();

            if (!context.TaskLists.Any())
            {
                Console.WriteLine("Adding data - seeding...");
                context.TaskLists.AddRange(
                    new TaskList
                    {
                        Name = "lista da tivit",
                        Tasks = new List<Models.Task>()
                        {
                            new Models.Task
                            {
                                Title = "fazer unit tests",
                                Priority = 2,
                                Status = "open"
                            },
                            new Models.Task
                            {
                                Title = "arranjar outro emprego",
                                Priority = 1,
                                Status = "open"
                            }
                        }
                    },
                    new TaskList
                    {
                        Name = "lista do btg",
                        Tasks = new List<Models.Task>()
                        {
                            new Models.Task
                            {
                                Title = "fazer entrevista",
                                Priority = 2,
                                Status = "open"
                            },
                            new Models.Task
                            {
                                Title = "ficar rico",
                                Priority = 1,
                                Status = "open"
                            }
                        }
                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Already have data - not seeding");
            }
        }
    }
}
