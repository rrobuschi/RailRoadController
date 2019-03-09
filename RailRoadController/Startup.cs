using System.IO.Ports;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailRoadController.BL.DccCommand;
using RailRoadController.BL.Locomotive;
using RailRoadController.BL.Track;
using RailRoadController.Entities;

namespace RailRoadController
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            services.Configure<MyAppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<ILocomotivePersister>(x =>
                new LocomotivePersister(Configuration.GetSection("AppSettings")["LocomotiveFile"]));
            services.AddSingleton<ITrackManager, TrackManager>();
            services.AddSingleton<ILocomotiveManager, LocomotiveManager>();
            services.AddTransient<ILocomotive, Locomotive>();
            services.AddTransient<IDccCommandBuilder, DccCommandBuilder>();
            if (CurrentEnvironment.IsDevelopment())
                services.AddTransient<IDccCommandSender, DccCommandSenderMock>();
            else
                services.AddTransient<IDccCommandSender, DccCommandSender>();

            services.AddSingleton<ISerialDevice>(x => new SerialDevice("/dev/ttyACM0", BaudRate.B115200));

            var serviceProvider = services.BuildServiceProvider();

            services.AddSingleton<ILocomotiveUpdateManager, LocomotiveUpdateManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default_route",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Engine", action = "Index" }
                );
                routes.MapRoute(name: "api", template: "api/{controller=Admin}");
            });
        }
    }
}
