using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChy.Frame.Core.Modules;
using MyChy.Frame.Core.Modules.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web5.Work
{
    public class AssemblyProvider : IAssemblyProvider
    {
        protected ILogger<AssemblyProvider> _logger;
        public IServiceProvider _serviceProvider;
        public Func<Assembly, bool> IsCandidateAssembly { get; set; }
        public Func<Library, bool> IsCandidateCompilationLibrary { get; set; }

        private static string DLLNamespace = "MyChy.";

        private static string DLLDateDomains = "MyChy.Frame.Core.Web";

        public AssemblyProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<AssemblyProvider>();
            this.IsCandidateAssembly = assembly =>
             /* !assembly.FullName.StartsWith("Microsoft.") && !assembly.FullName.Contains("SF.WebHost") &&*/ assembly.FullName.StartsWith(DLLDateDomains);
            this.IsCandidateCompilationLibrary = library =>
              library.Name != "NETStandard.Library" && !library.Name.StartsWith("Microsoft.") && !library.Name.StartsWith("System.");
        }

        public IEnumerable<Assembly> GetAssemblies(string path)
        {
            List<Assembly> assemblies = new List<Assembly>();

            assemblies.AddRange(this.GetAssembliesFromPath(path));
            //  assemblies.AddRange(this.GetAssembliesFromDependencyContext());
            return assemblies;
        }

        public IEnumerable<ModuleInfo> GetModules(string path)
        {

            IList<ModuleInfo> modules = new List<ModuleInfo>();

            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                this._logger.LogInformation("Discovering and loading assemblies from path '{0}'", path);
                var moduleRootFolder = new DirectoryInfo(path);
                var moduleFolders = moduleRootFolder.GetDirectories();
                var builder = new JsonModuleBuilder(_serviceProvider);
                foreach (var moduleFolder in moduleFolders)
                {
                    var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
                    if (!binFolder.Exists)
                    {
                        continue;
                    }

                    foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                    {
                        Assembly assembly;
                        try
                        {
                            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                        }
                        catch (FileLoadException)
                        {
                            // Get loaded assembly
                            assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                            if (assembly == null)
                            {
                                throw;
                            }
                        }

                        //过滤非主模块库，如SF.Module.Backend.Data
                        if (assembly.FullName.Split(',')[0].Equals(moduleFolder.Name))
                        {
                            var moduleInfo = new ModuleInfo
                            {
                                Name = moduleFolder.Name,
                                Assembly = assembly,
                                Path = moduleFolder.FullName,

                            };
                            moduleInfo.Config = builder.BuildConfig(moduleFolder.FullName).Result ?? new ModuleConfig();

                            modules.Add(moduleInfo);
                        }
                    }
                }
            }


            return modules;

        }

        private IEnumerable<Assembly> GetAssembliesFromPath(string path)
        {
            List<Assembly> assemblies = new List<Assembly>();
            var binFolder = new DirectoryInfo(path);
            if (!binFolder.Exists)
            {
                this._logger.LogWarning("Discovering and loading assemblies from path '{0}' skipped: path not found", binFolder);
                return assemblies;
            }
            this._logger.LogInformation("Discovering and loading assemblies from path '{0}'", binFolder);
            foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
            {
                if (!CheckNamespace(file.Name))
                {
                    continue;
                }
                Assembly assembly;
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                }
                catch (FileLoadException)
                {
                    // Get loaded assembly
                    assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                    if (assembly == null)
                    {
                        throw;
                    }
                }
                if (this.IsCandidateAssembly(assembly) && !assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                    this._logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
                }

            }


            return assemblies;
        }

        private bool CheckNamespace(string filename)
        {
            var reuslt = false;
            if (filename.Contains(DLLNamespace))
            {
                reuslt = true;
            }
            return reuslt;

        }

        private IEnumerable<Assembly> GetAssembliesFromDependencyContext()
        {
            List<Assembly> assemblies = new List<Assembly>();

            this._logger.LogInformation("Discovering and loading assemblies from DependencyContext");

            foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
            {
                if (this.IsCandidateCompilationLibrary(compilationLibrary))
                {
                    Assembly assembly = null;

                    try
                    {
                        assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
                        assemblies.Add(assembly);
                        this._logger.LogInformation("Assembly '{0}' is discovered and loaded", assembly.FullName);
                    }

                    catch (Exception e)
                    {
                        this._logger.LogWarning("Error loading assembly '{0}'", compilationLibrary.Name);
                        this._logger.LogInformation(e.ToString());
                    }
                }
            }

            return assemblies;
        }

    }
}
