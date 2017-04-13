using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Abstraction.Interceptors
{
    /// <summary>
    /// 拦截接口
    /// </summary>
    public interface IInterceptor
    {
        void Before(InterceptionContext context);
        void After(InterceptionContext context);
    }
}
