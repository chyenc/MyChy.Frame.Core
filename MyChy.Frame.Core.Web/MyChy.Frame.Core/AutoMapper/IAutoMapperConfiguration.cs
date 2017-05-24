using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.AutoMapper
{
    /// <summary>
    /// AutoMapper配置注册
    /// </summary>
    public interface IAutoMapperConfiguration
    {
        void MapperConfigurationToExpression(IMapperConfigurationExpression cfg);
    }
}
