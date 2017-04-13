﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyChy.Frame.Core.Common.Core.Modules
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public Assembly Assembly { get; set; }

        public string ShortName
        {
            get
            {
                return Name.Split('.').Last();
            }
        }

        public string Path { get; set; }
    }
}
