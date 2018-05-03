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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MyChy.Frame.Core.Common.Helper;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using MyChy.Frame.Core.Common.Extensions;

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
            //RedisConfig

            //var name = "aa.jpge";
            //var ss = FileHelper.CheckFileNmae(name, out string namex, out string exname);
            //name = "aa.jpg.jpge.";
            //ss = FileHelper.CheckFileNmae(name, out namex, out exname);

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

                var Version = efconfig.Version.To<int>(0);
                if (Version > 2008)
                {
                    services.AddEntityFrameworkSqlServer()
                    //################################################
                    //它显示的使用了一种实例池的方式来注入到容器。
                    .AddDbContextPool<CoreDbContext>((serviceProviders, options) =>
                    //################################################
                    //.AddDbContext<CoreDbContext>((serviceProviders, options) =>
                    options.UseSqlServer(efconfig.Connect,
                            b => b.MigrationsAssembly("MyChy.Web"))
                           .UseInternalServiceProvider(serviceProviders));
                    // .UseRowNumberForPaging() SQL2008版本需要，12等以上版本不需要
                }
                else
                {
                    services.AddEntityFrameworkSqlServer()
                    .AddDbContextPool<CoreDbContext>((serviceProviders, options) =>
                    options.UseSqlServer(efconfig.Connect,
                            b => b.MigrationsAssembly("MyChy.Web").UseRowNumberForPaging())
                           .UseInternalServiceProvider(serviceProviders));
                    //.UseRowNumberForPaging() SQL2008版本需要，12等以上版本不需要

                }

                // .UseRowNumberForPaging() ＳＱＬ２００８　增加　１２等版本不需要

            }
            // services.AddSingleton<CoreDbContext>();
            services.AddSingleton<DbContext, CoreDbContext>();


            services.AddHttpContextAccessor();

            services.AddSession();

            services.AddTransient<ICompetencesWorkArea, CompetencesWorkArea>();

            services.AddTransient<IBaseUnitOfWork, BaseUnitOfWork>();


            var machinekeyPath = FileHelper.GetFileMapPath("config");
            var protectionProvider = DataProtectionProvider.Create(new DirectoryInfo(machinekeyPath));
            var dataProtector = protectionProvider.CreateProtector("MyCookieAuthentication");
            var ticketFormat = new TicketDataFormat(dataProtector);



            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
                options.TicketDataFormat = ticketFormat;
            });
           // appconfig = _Configuration.GetSection("AppSettings").Get<AppSettingsConfig>();


            // Add framework services.
            services.AddMvc();

            //if (appconfig.StartupTask)
            //{
            //    var serviceProvider = services.BuildServiceProvider();
            //    CoreEFStartupTask task = new CoreEFStartupTask(serviceProvider);
            //    task.RunS();
            //}


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

            app.UseAuthentication();

            app.UseSession();

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
