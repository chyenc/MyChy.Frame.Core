using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.Entitys.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    //public class EFCoreBaseRepository<TEntity> : 
    //    EFCoreQueryableRepository<TEntity, int> where TEntity : EntityWithTypedId<int>
    //{
    //    public EFCoreBaseRepository(DbContext context) : base(context)
    //    {
    //    }
    //    public override IQueryable<TEntity> QueryById(int id)
    //    {
    //        return Query().Where(e => e.Id == id);
    //    }

    //    public override IQueryable<TEntity> QueryByIdNoTracking(int id)
    //    {
    //        return QueryNoTracking().Where(e => e.Id == id);
    //    }

    //}

    //public class EFCoreBaseRepository<TEntity, TKey> :
    //    EFCoreQueryableRepository<TEntity, TKey> where TEntity : EntityWithTypedId<TKey>
    //{
    //    public EFCoreBaseRepository(DbContext context) : base(context)
    //    {
    //    }
    //    public override IQueryable<TEntity> QueryById(TKey id)
    //    {
    //        return Query().Where(e => e.Id.Equals(id));
    //    }

    //    public override IQueryable<TEntity> QueryByIdNoTracking(TKey id)
    //    {
    //        return QueryNoTracking().Where(e => e.Id.Equals(id));
    //    }

    //}
}
