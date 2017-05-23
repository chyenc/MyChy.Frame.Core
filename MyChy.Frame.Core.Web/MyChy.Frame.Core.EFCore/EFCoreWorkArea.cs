using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore
{
    /// <summary>
    /// Represents a work area that can be used for aggregating
    /// UoW properties, specialized for the Entity Framework Core
    /// </summary>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    public abstract class EFCoreWorkArea<TDbContext> : IEFCoreWorkArea<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The database context</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected EFCoreWorkArea(TDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        #region Implementation of IEFCoreWorkArea<out TDbContext>

        /// <summary>
        /// The Entity Framework database context
        /// </summary>
        public TDbContext Context { get; }

        #endregion
    }

    /// <summary>
    /// Represents a work area that can be used for aggregating
    /// UoW properties, specialized for the Entity Framework Core
    /// </summary>
    public abstract class EFCoreWorkArea : EFCoreWorkArea<DbContext>, IEFCoreWorkArea
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The database context</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected EFCoreWorkArea(DbContext context) : base(context)
        {


        }
    }
}
