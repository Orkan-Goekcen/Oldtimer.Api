using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Controller;
using Oldtimer.Api.Data;
using Oldtimer.Api.Examples;
using Oldtimer.Api.Queries;
using Swashbuckle.AspNetCore.Filters;

namespace Oldtimer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            //builder.Services.AddScoped<IApiService, ApiService>();

            builder.Services.AddDbContext<ApiContext>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.ExampleFilters();
            });

            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
            builder.Services.AddSingleton<OldTimerExample>();
            builder.Services.AddSingleton<IExamplesProvider<Sammler>, SammlerExample>();
            builder.Services.AddSingleton<Sammler>();



            builder.Services.AddValidatorsFromAssemblyContaining<OldtimerApiController>();
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(GetAllOldtimerQuery).Assembly);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var ctx = services.GetRequiredService<ApiContext>();
                ctx.Database.Migrate();

                if (app.Environment.IsDevelopment())
                {
                    ctx.SeedInitialData();
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.Run();
        }
    }
}