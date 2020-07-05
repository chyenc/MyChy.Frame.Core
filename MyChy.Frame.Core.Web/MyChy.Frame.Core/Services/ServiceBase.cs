using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Services
{
    /// <summary>
    /// 基础服务基类
    /// </summary>
    public abstract class ServiceBase : IServiceBase
    {
        //private readonly IMapper mapper;

        public virtual void Dispose()
        {


        }
    }
}
