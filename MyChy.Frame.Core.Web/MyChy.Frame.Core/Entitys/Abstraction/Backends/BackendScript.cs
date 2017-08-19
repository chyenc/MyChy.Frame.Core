using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Entitys.Abstraction.Backends
{
    public class BackendScript
    {
        public string Url { get; set; }
        public int Position { get; set; }

        public BackendScript(string url, int position)
        {
            this.Url = url;
            this.Position = position;
        }
    }
}
