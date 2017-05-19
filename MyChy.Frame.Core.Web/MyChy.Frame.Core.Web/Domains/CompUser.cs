using MyChy.Frame.Core.EFCore.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web.Domains
{
    public class CompUser : BaseWithAllEntity
    {
        ///<summary>
        ///昵称
        ///</summary>
        [StringLength(50)]
        [Description("昵称")]
        public string NickName { get; set; }

        ///<summary>
        ///用户名
        ///</summary>
        [StringLength(50)]
        [Description("用户名")]
        public string UserName { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [StringLength(50)]
        [Description("密码")]
        public string PassWord { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        [Description("状态")]
        public int State { get; set; }


        /// <summary>
        /// 一个用户一个角色
        /// </summary>
        //[ForeignKey("CompRole")]
        // public int RoleId { get; set; }

        //public virtual CompRole CompRole { get; set; }

    }
}
