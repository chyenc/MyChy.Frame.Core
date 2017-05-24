using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Entitys.Abstraction
{
    public interface IEntityWithTypedId<TId> : IEntity
    {
        TId Id { get; set; }
    }

    public interface IEntity
    {

    }
}
