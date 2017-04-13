using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Represents an Unit of Work specialized for the Entity Framework Core.
    /// </summary>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    public interface IEFCoreUnitOfWork<out TDbContext> : IUnitOfWork
        where TDbContext : DbContext
    {
        /// <summary>
        /// The Entity Framework database context
        /// </summary>
        TDbContext Context { get; }
    }

    /// <summary>
    /// Represents an Unit of Work specialized for the Entity Framework Core.
    /// </summary>
    public interface IEFCoreUnitOfWork : IEFCoreUnitOfWork<DbContext>
    {

    }
}
