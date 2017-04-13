using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Entitys.Attributes
{
    /// <summary>
    /// Tells the auditing engine to NOT audit this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MapIgnoreAttribute : Attribute
    {

    }
}
