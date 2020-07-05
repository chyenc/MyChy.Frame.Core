using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChy.Frame.Core.Entitys.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Modules
{
    public abstract class ModuleInitializerBase : IModuleInitializer
    {
        protected IWebHostEnvironment  webHostEnvironment;
        protected IServiceProvider serviceProvider;
        protected IConfigurationRoot configurationRoot;
        protected ILogger<ModuleInitializerBase> logger;

        public virtual IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities
        {
            get
            {
                return null; 
            }
        }

        public virtual IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities
        {
            get
            {
                return null;
            }
        }

        public virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.webHostEnvironment = serviceProvider.GetService<IWebHostEnvironment>();
            this.logger = this.serviceProvider.GetService<ILoggerFactory>().CreateLogger<ModuleInitializerBase>();
        }

        public virtual void SetConfigurationRoot(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }
        public virtual IBackendMetadata BackendMetadata
        {
            get
            {
                return null;
            }
        }

        public virtual IEnumerable<KeyValuePair<int, Action<IMvcBuilder>>> AddMvcActionsByPriorities
        {
            get
            {
                return null;

            }

        }

        public virtual IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities
        {
            get
            {
                return null;
                //return new Dictionary<int, Action<IRouteBuilder>>()
                //{
                //    [2000] = routeBuilder =>
                //    {
                //        routeBuilder.MapRoute(name: "Extension B", template: "extension-b", defaults: new { controller = "ExtensionB", action = "Index" });
                //    }
                //};
            }
        }

    }
}
