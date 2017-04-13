using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.Entitys.Abstraction;
using MyChy.Frame.Core.EFCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.EFCore
{
    /// <summary>
    /// 基础仓储接口
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public interface IBaseRepository<Entity> : 
        IEFCoreQueryableRepository<Entity, int> where Entity : EntityWithTypedId<int>
    {

    }

    /// <summary>
    /// 基础仓储接口
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public interface IBaseRepository<Entity, TKey> :
        IEFCoreQueryableRepository<Entity, TKey> where Entity : EntityWithTypedId<TKey>
    {


    }

    /// <summary>
    /// 基础仓库类
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class BaseRepository<Entity> :
        EFCoreQueryableRepository<Entity, int>, IBaseRepository<Entity> 
        where Entity : EntityWithTypedId<int>
    {
        public BaseRepository(DbContext context) : base(context)
        {

        }
        public override IQueryable<Entity> QueryById(int id)
        {
            return Query().Where(e => e.Id == id);
        }
    }


    /// <summary>
    /// 基础仓库类
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class BaseRepository<Entity, TKey> :
        EFCoreQueryableRepository<Entity, TKey>, IBaseRepository<Entity, TKey> 
        where Entity : EntityWithTypedId<TKey>
    {
        public BaseRepository(DbContext context) : base(context)
        {

        }
        public override IQueryable<Entity> QueryById(TKey id)
        {
            return Query().Where(e => e.Id.Equals(id));
        }
    }
}
