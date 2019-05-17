using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MyChy.Frame.Core.EFCore.Extensions
{
   //public static class DatabaseSqlExtensions
   // {
   //     public static async Task<DbDataReader> ExecuteReaderAsync(RawSqlString sql, params object[] parameters)
   //     {
   //         using (await Database.GetService<IConcurrencyDetector>().EnterCriticalSectionAsync(default))
   //         {
   //             RawSqlCommand rawSqlCommand = Database.GetService<IRawSqlCommandBuilder>().Build(sql.Format, parameters);
   //             return (await rawSqlCommand.RelationalCommand.ExecuteReaderAsync(Database.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues)).DbDataReader;
   //         }
   //     }
   //     //  FormattableString
   //     public static Task<DbDataReader> ExecuteReaderAsync(FormattableString sql)
   //     {
   //         return ExecuteReaderAsync(sql.Format, sql.GetArguments());
   //     }
   //     public static Task<object> ExecuteScalarAsync(FormattableString sql)
   //     {
   //         return ExecuteScalarAsync(sql.Format, sql.GetArguments());
   //     }
   //     public static async Task<object> ExecuteScalarAsync(RawSqlString sql, params object[] parameters)
   //     {
   //         using (await Database.GetService<IConcurrencyDetector>().EnterCriticalSectionAsync(default))
   //         {
   //             RawSqlCommand rawSqlCommand = Database.GetService<IRawSqlCommandBuilder>().Build(sql.Format, parameters);
   //             return await rawSqlCommand.RelationalCommand.ExecuteScalarAsync(Database.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues);
   //         }
   //     }
   // }
}
