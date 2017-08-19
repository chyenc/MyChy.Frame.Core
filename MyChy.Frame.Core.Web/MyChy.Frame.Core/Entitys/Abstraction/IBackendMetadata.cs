using MyChy.Frame.Core.Entitys.Abstraction.Backends;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Entitys.Abstraction
{
   public  class IBackendMetadata
    {
        IEnumerable<BackendStyleSheet> BackendStyleSheets { get; }
        IEnumerable<BackendScript> BackendScripts { get; }
        IEnumerable<BackendMenuGroup> BackendMenuGroups { get; }
    }
}
