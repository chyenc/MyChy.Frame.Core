using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Entitys.Abstraction
{
    public interface IEntityWithTypedId<TId> : IEntity
    {
        TId Id { get; set; }
    }

    public interface IEntity
    {

    }
}
