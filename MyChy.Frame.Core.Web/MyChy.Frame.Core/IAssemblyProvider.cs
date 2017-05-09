using MyChy.Frame.Core.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyChy.Frame.Core
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies(string path);

        IEnumerable<ModuleInfo> GetModules(string path);

    }
}
