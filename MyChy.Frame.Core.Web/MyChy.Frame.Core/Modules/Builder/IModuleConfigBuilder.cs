using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Modules.Builder
{
    public interface IModuleConfigBuilder
    {
        Task<ModuleConfig> BuildConfig(string filePath);
    }
}
