using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Models.ViewModels;
using PerfectPoliciesFE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE
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
            services.AddControllersWithViews();

            services.AddDistributedMemoryCache(); // to store session data in server's memory.

            // register session to be used by the application
            services.AddSession(c => {

                // session name cookie
                c.Cookie.Name = "UserSession";
                c.Cookie.HttpOnly = true;  // to not be accessed by javaScript
                c.Cookie.IsEssential = true;
                c.IdleTimeout = TimeSpan.FromMinutes(30); // define session timeOut duration

            });

            // regisering services to be injectedin controllers.
            //services.AddScoped<QuizServices>();
            services.AddScoped <IAPIRequest<Quiz>,APIRequest<Quiz>>();
            services.AddScoped<IAPIRequest<Question>,APIRequest<Question>>();
            services.AddScoped<IAPIRequest<Option>,APIRequest<Option>>();
            services.AddScoped<IAPIRequest<QuestionsPerQuizViewModel>, APIRequest<QuestionsPerQuizViewModel>>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();  // used to allow APIRequest class access API web service and access session values.

            // register API 
            services.AddHttpClient("APIClient", c => {

                c.BaseAddress = new Uri(Configuration["WebAPIURL"]);
                c.DefaultRequestHeaders.Clear();
                c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); 
            }
            );
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();  // use authorization middleWare.

            app.UseSession();  // use session middleWare.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
