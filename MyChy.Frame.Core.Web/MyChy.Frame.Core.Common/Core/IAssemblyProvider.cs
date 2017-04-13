using MyChy.Frame.Core.Common.Core.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyChy.Frame.Core.Common.Core
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies(string path);


        IEnumerable<ModuleInfo> GetModules(string path);
    }
}
