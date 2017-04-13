using System;
using Microsoft.Extensions.Configuration;

namespace MyChy.Frame.Core.Common.Helper
{
    public class ConfigHelper
    {
        public T Reader<T>(string fileConfig) where T : class, new()
        {
            var configurationBuilder = new ConfigurationBuilder();
            try
            {
                var file = FileHelper.GetFileMapPath(fileConfig);
                configurationBuilder.AddJsonFile(file);
                return configurationBuilder.Build().Get<T>();
            }
            catch (Exception exception)
            {
                LogHelper.Log(exception);
                return default(T);
            }
        }
    }
}
