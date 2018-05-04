using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.AutoHistorys.Internal
{
    internal class AutoHistoryConfig
    {
        public bool IsHistory { get; set; }

        public bool IsAutoHistory { get; set; }

        public string TypeName { get; set; }

        public bool IsAdded { get; set; }


        public bool OperatorIsLogin { get; set; }

    }
}
