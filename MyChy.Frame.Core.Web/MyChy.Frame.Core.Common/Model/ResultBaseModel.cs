using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Common.Model
{
    public class ResultBaseModel
    {
        public bool Success { get; set; }

        public string Msg { get; set; }

        public string Code { get; set; }

        public int Id { get; set; }
    }
}
