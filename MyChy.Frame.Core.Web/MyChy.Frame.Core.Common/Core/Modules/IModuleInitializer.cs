﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Common.Core.Modules
{
    public interface IModuleInitializer
    {

        ///// <summary>
        ///// 服务注册
        ///// </summary>
        //IEnumerable<KeyValuePair<int, Action<IServiceCollection>>> ConfigureServicesActionsByPriorities { get; }
        ///// <summary>
        ///// 配置构建
        ///// </summary>
        //IEnumerable<KeyValuePair<int, Action<IApplicationBuilder>>> ConfigureActionsByPriorities { get; }

        //void SetServiceProvider(IServiceProvider serviceProvider);

        //void SetConfigurationRoot(IConfigurationRoot configurationRoot);

        ///// <summary>
        ///// 添加模块MVC配置
        ///// </summary>
        //IEnumerable<KeyValuePair<int, Action<IMvcBuilder>>> AddMvcActionsByPriorities { get; }
       
        ///// <summary>
        ///// 自定义模块路由
        ///// </summary>
        //IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities { get; }

        ///// <summary>
        ///// 后台元数据,包括css,js,后台菜单，模块权限等
        ///// </summary>
        //IBackendMetadata BackendMetadata { get; }
    }
}
