using System;
using MyChy.Frame.Core.EFCore.Config;
using MyChy.Frame.Core.Common.Helper;

namespace MyChy.Frame.Core.EFCore
{
    public class EntityFrameworkHelper
    {
         public static EntityFrameworkConfig ReadConfiguration(string file= "config/EntityFramework.json")
        {
            var config = new ConfigHelper();
            var entityconfig = new EntityFrameworkConfig();
            try
            {
                entityconfig = config.Reader<EntityFrameworkConfig>(file);
                switch (entityconfig.BaseType.ToLower())
                {
                    case "mssql":
                        entityconfig.SqlType = EntityFrameworkType.MsSql;
                        break;
                    case "mysql":
                        entityconfig.SqlType = EntityFrameworkType.MySql;
                        break;
                    case "oracle":
                        entityconfig.SqlType = EntityFrameworkType.Oracle;
                        break;
                    case "sqlite":
                        entityconfig.SqlType = EntityFrameworkType.Sqlite;
                        break;
                    case "":
                        entityconfig.SqlType = EntityFrameworkType.Null;
                        break;
                    default:
                        entityconfig.SqlType = EntityFrameworkType.MsSql;
                        break;
                }
                if (string.IsNullOrEmpty(entityconfig.Connect))
                {
                    entityconfig.SqlType = EntityFrameworkType.Null;
                }
            }
            catch (Exception exception)
            {
                entityconfig = new EntityFrameworkConfig
                {
                    SqlType = EntityFrameworkType.Null
                };
                LogHelper.Log(exception);
            }
             return entityconfig;
             //finally
             //{
             //    Config.IsCache = false;
             //    IsCacheError = true;
             //}

        }
    }
}
