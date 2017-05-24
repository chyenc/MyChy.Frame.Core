using MyChy.Frame.Core.Entitys.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Entitys.Abstraction
{

    public abstract class EntityWithTypedId : EntityWithTypedId<int>
    {



    }

    public abstract class EntityWithTypedId<TId> :  IEntityWithTypedId<TId>
    {
        public TId Id { get; set; }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        protected EntityWithTypedId()
        {

        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="id">The id for the entity</param>
        protected EntityWithTypedId(TId id)
        {
            Id = id;
        }
    }
}
