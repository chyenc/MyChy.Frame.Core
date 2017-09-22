using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Entitys.Abstraction
{
    /// <summary>
    /// Metadata information about the entity soft deleted
    /// </summary>
    /// <typeparam name="TDeletedBy">The identifier or entity type</typeparam>
    public interface IHaveDeletedMeta<TDeletedBy>
    {
        /// <summary>
        /// The <see cref="DateTimeOffset"/> when it was soft deleted
        /// </summary>
        DateTimeOffset? DeletedOn { get; set; }

        /// <summary>
        /// The identifier (or entity) which soft deleted this entity
        /// </summary>
        TDeletedBy DeletedBy { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted{ get; set; }
    }

    /// <summary>
    /// Metadata information about the entity deletition, using a <see cref="string"/>
    /// as an identifier for the <see cref="IHaveDeletedMeta{T}.DeletedBy"/>
    /// </summary>
    public interface IHaveDeletedMeta : IHaveDeletedMeta<string>
    {

    }
}
