using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Common.Model
{
    public class ResultBaseModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
    }
}
