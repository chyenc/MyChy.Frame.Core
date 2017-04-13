using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.AutoHistorys.Internal
{
    public class EventEntryChange
    {
        [JsonProperty(Order = 10)]
        public string ColumnName { get; set; }
        [JsonProperty(Order = 20)]
        public object OriginalValue { get; set; }
        [JsonProperty(Order = 30)]
        public object NewValue { get; set; }

    }
}
