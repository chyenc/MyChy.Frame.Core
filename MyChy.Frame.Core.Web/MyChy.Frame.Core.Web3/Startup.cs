using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyChy.Frame.Core.Common.Extensions;
using MyChy.Frame.Core.Common.Helper;
using MyChy.Frame.Core.EFCore;
using MyChy.Frame.Core.EFCore.Config;
using MyChy.Frame.Core.Extensions;
using MyChy.Frame.Core.Modules;
using MyChy.Frame.Core.StartupTask;
using MyChy.Frame.Core.Web.Work;
using NLog.Extensions.Logging;

namespace MyChy.Frame.Core.Web3
{
    public class Startup
    {
        protected IServiceProvider serviceProvider;
        protected IAssemblyProvider assemblyProvider;
        protected ILogger<Startup> logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            serviceProvider = services.BuildServiceProvider();
            assemblyProvider = new AssemblyProvider(serviceProvider);
            logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Startup>();

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
                            b => b.MigrationsAssembly("MyChy.Frame.Core.Web3"))
                           .UseInternalServiceProvider(serviceProviders));
                    // .UseRowNumberForPaging() SQL2008版本需要，12等以上版本不需要
                }
                else
                {
                    services.AddEntityFrameworkSqlServer()
                    .AddDbContextPool<CoreDbContext>((serviceProviders, options) =>
                    options.UseSqlServer(efconfig.Connect,
                            b => b.MigrationsAssembly("MyChy.Frame.Core.Web3").UseRowNumberForPaging())
                           .UseInternalServiceProvider(serviceProviders));
                    //.UseRowNumberForPaging() SQL2008版本需要，12等以上版本不需要

                }

                //.UseRowNumberForPaging() ＳＱＬ２００８　增加　１２等版本不需要

            }
            else if (efconfig.SqlType == EntityFrameworkType.MySql)
            {
                services.AddEntityFrameworkMySql()
                .AddDbContextPool<CoreDbContext>((serviceProviders, options) =>
                options.UseMySql(efconfig.Connect,
                   b => b.MigrationsAssembly("MyChy.Frame.Core.Web3"))
                  .UseInternalServiceProvider(serviceProviders));

            }
            // services.AddSingleton<CoreDbContext>();
            services.AddSingleton<DbContext, CoreDbContext>();


            services.AddHttpContextAccessors();

            services.AddSession();

            services.AddTransient<ICompetencesWorkArea, CompetencesWorkArea>();

            services.AddTransient<IBaseUnitOfWork, BaseUnitOfWork>();

            services.AddAuthentication()
                    .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
               // options.TicketDataFormat = ticketFormat;
            }); ;

            //var machinekeyPath = FileHelper.GetFileMapPath("config");
            //var protectionProvider = DataProtectionProvider.Create(new DirectoryInfo(machinekeyPath));
            //var dataProtector = protectionProvider.CreateProtector("MyCookieAuthentication");
            //var ticketFormat = new TicketDataFormat(dataProtector);

            //services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(options =>
            //{
            //    options.LoginPath = new PathString("/Account/Login");
            //    options.LogoutPath = new PathString("/Account/Logout");
            //    options.TicketDataFormat = ticketFormat;
            //});
            // appconfig = _Configuration.GetSection("AppSettings").Get<AppSettingsConfig>();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});



            // Add framework services.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0); ;


            services.AddRazorPages();

            // if (appconfig.IsNlog)
            // {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddNLog("config/nlog.config");
            });
            // }

            //if (appconfig.StartupTask)
            //{
                 serviceProvider = services.BuildServiceProvider();
                CoreEFStartupTask task = new CoreEFStartupTask(serviceProvider);
                task.RunS();
           // }

            //  services.AddSingleton<ISFStarter, StartupTaskStarter>();

            //     // ConfigureServicesTask(services);

            //      /serviceProvider = services.BuildServiceProvider();

            //      var sfStarter = serviceProvider.GetService<ISFStarter>();

            //      sfStarter.Run();

            ////  }

            //serviceProvider = services.BuildServiceProvider();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //安装NuGet包 Session
            app.UseSession();

            app.UseStaticHttpContext();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
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
                null : this.serviceProvider.GetService<IWebHostEnvironment>().ContentRootPath
            );
            ExtensionManager.SetAssemblies(assemblies.ToList());

            IEnumerable<ModuleInfo> modules = this.assemblyProvider.GetModules(
              string.IsNullOrEmpty(extensionsPath) ?
                null : this.serviceProvider.GetService<IWebHostEnvironment>().ContentRootPath + extensionsPath);

            //ExtensionManager.SetModules(modules.ToList());

        }
    }
}
