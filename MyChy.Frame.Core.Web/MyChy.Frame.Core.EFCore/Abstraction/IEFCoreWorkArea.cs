using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Abstraction
{
    /// <summary>
    /// Represents a work area that can be used for aggregating
    /// UoW properties, specialized for the Entity Framework Core
    /// </summary>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    public interface IEFCoreWorkArea<out TDbContext> : IWorkArea
        where TDbContext : DbContext
    {
        /// <summary>
        /// The Entity Framework database context
        /// </summary>
        TDbContext Context { get; }
    }

    /// <summary>
    /// Represents a work area that can be used for aggregating
    /// UoW properties, specialized for the Entity Framework Core
    /// </summary>
    public interface IEFCoreWorkArea : IEFCoreWorkArea<DbContext>
    {

    }
}
