using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Abstraction
{
    /// <summary>
    /// 自定义实体映射
    /// 模块如支持多数据库，请不要继承该接口
    /// </summary>
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}
