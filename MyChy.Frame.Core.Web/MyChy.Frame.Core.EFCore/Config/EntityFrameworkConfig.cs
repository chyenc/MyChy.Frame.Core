using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.EFCore.Config
{
    public class EntityFrameworkConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public EntityFrameworkType SqlType { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string BaseType { get; set; }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        public string Connect { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Overtime { get; set; }

        /// <summary>
        /// 连接串是否加密
        /// </summary>
        public bool IsEncryption { get; set; }

        /// <summary>
        /// 加密KEY
        /// </summary>
        public string Encryption { get; set; }
    }

    public enum EntityFrameworkType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        Null = 0,
        /// <summary>
        /// MsSqL
        /// </summary>
        [Description("MsSql")]
        MsSql = 1,
        /// <summary>
        /// MySql
        /// </summary>
        [Description("MySql")]
        MySql = 2,
        /// <summary>
        /// Oracle
        /// </summary>
        [Description("Oracle")]
        Oracle = 3,
        /// <summary>
        /// Sqlite 
        /// </summary>
        [Description("Sqlite")]
        Sqlite = 4,
    }
}
