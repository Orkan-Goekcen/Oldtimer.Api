//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Oldtimer.Api.Data;
//using Oldtimer.Api.Service;

//namespace Oldtimer.Api
//{
//    public class Startup
//    {
//        public IConfiguration Configuration { get; }

//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddRazorPages();

//            services.AddDbContext<ApiContext>(options =>
//            {
//                options.UseSqlite(Configuration.GetConnectionString("SammlerDbFilename"));
//            });

//            services.AddScoped<IApiService, ApiService>();
//            services.AddControllers();
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthorization();

//            // Initialize and seed the database
//            //using (var scope = app.ApplicationServices.CreateScope())
//            //{
//            //    var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
//            //    context.Database.EnsureCreated();
//            //    context.SeedInitialData();
//            //}

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapRazorPages();
//                endpoints.MapControllers();
//            });
//        }
//    }
//}
