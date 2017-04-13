using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Attributes
{
    /// <summary>
    /// Used with OptIn AnnotationMode to include the entity on the Audit logs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AuditIncludeAttribute : Attribute
    {

    }
}
