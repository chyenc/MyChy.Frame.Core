using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using MyChy.Frame.Core.Modules;
using MyChy.Frame.Core.EFCore;
using MyChy.Frame.Core.EFCore.Config;
using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.Web.Work;
using MyChy.Frame.Core.Extensions;

namespace MyChy.Frame.Core.Web
{
    public class Startup
    {
        protected IServiceProvider serviceProvider;
        private readonly IHostingEnvironment _hostingEnvironment;
        protected IAssemblyProvider assemblyProvider;
        protected ILogger<Startup> logger;

        public Startup(IHostingEnvironment env, IServiceProvider serviceProvider)
: this(env, serviceProvider, new AssemblyProvider(serviceProvider))
        {
            this.logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Startup>();
        }

        public Startup(IHostingEnvironment env, IServiceProvider _serviceProvider, IAssemblyProvider _assemblyProvider)
        {
            _hostingEnvironment = env;
            serviceProvider = _serviceProvider;
            assemblyProvider = _assemblyProvider;

            var builder = new ConfigurationBuilder()
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
            //var builder = startupbase.Startup(env, _serviceProvider, _assemblyProvider);
            Configuration = builder.Build();
        }

        //public Startup(IHostingEnvironment env)
        //{

        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //}

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DiscoverAssemblies();

            var efconfig = EntityFrameworkHelper.ReadConfiguration("config/EntityFramework.json");
            //services.AddDbContext<DataContext>(options => options.UseSqlite(connection));
            if (efconfig.SqlType == EntityFrameworkType.MsSql)
            {
                //services.AddEntityFrameworkSqlServer().
                //AddDbContext<MySqlDbContext>(options => options.UseSqlServer(efconfig.Connect))
                //  // .UseInternalServiceProvider(services)
                //  ;
                //var contextOptions1 = new DbContextOptionsBuilder<MySqlDbContext>()
                //.UseSqlServer(efconfig.Connect)
                //.Options;

                //services.AddSingleton(contextOptions1).AddScoped<MySqlDbContext>();


                services.AddEntityFrameworkSqlServer()
               .AddDbContext<CoreDbContext>((serviceProviders, options) =>
               options.UseSqlServer(efconfig.Connect,
                    b => b.MigrationsAssembly("MyChy.Frame.Core.Web"))
                      .UseInternalServiceProvider(serviceProviders));

            }
           // services.AddSingleton<CoreDbContext>();
            services.AddSingleton<DbContext, CoreDbContext>();


            services.AddHttpContextAccessor();

            services.AddSession();

            // Add framework services.
            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();
            CoreEFStartupTask task = new CoreEFStartupTask(serviceProvider);
            task.RunS();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseStaticHttpContext();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



        }

        /// <summary>
        /// 加载安装的模块信息
        /// </summary>
        private void DiscoverAssemblies()
        {
            string extensionsPath = this.Configuration["Modules:Path"];
            IEnumerable<Assembly> assemblies = this.assemblyProvider.GetAssemblies(
              string.IsNullOrEmpty(extensionsPath) ?
                null : this.serviceProvider.GetService<IHostingEnvironment>().ContentRootPath
            );
            ExtensionManager.SetAssemblies(assemblies.ToList());

            IEnumerable<ModuleInfo> modules = this.assemblyProvider.GetModules(
              string.IsNullOrEmpty(extensionsPath) ?
                null : this.serviceProvider.GetService<IHostingEnvironment>().ContentRootPath + extensionsPath);

            //ExtensionManager.SetModules(modules.ToList());

        }
    }
}
