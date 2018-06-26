using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyChy.Frame.Core.Common.Helper;
using MyChy.Frame.Core.EFCore.Attributes;
using MyChy.Frame.Core.EFCore.AutoHistorys.Internal;
using MyChy.Frame.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyChy.Frame.Core.EFCore.AutoHistorys
{
    /// <summary>
    /// Represents a plugin for Microsoft.EntityFrameworkCore to support automatically recording data changes history.
    /// </summary>
    public static class DbContextAutoHistoryExtensions
    {
        #region Private fields
        // Entities Include/Ignore attributes cache
        private static readonly Dictionary<Type, bool?> EntitiesIncludeIgnoreAttrCache = new Dictionary<Type, bool?>();

        private static readonly AutoHistoryConfig autoHistoryConfig;

        // private static readonly bool IsAutoHistory = EntityFrameworkHelper.ReadConfiguration().AutoHistory;

        static DbContextAutoHistoryExtensions()
        {
            autoHistoryConfig = ReadConfig();
        }

        #endregion
        /// <summary>
        /// Ensures the automatic history.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void EnsureAutoHistory(this DbContext context, string Operator = "SyStem", string FullName = "")
        {
            if (!autoHistoryConfig.IsHistory) return;

            // TODO: only record the changed properties.
            var jsonSetting = new JsonSerializerSettings
            {
                ContractResolver = new EntityContractResolver(context),
            };

            var entries = new List<EntityEntry>();
            if (autoHistoryConfig.IsAdded)
            {
                entries = context.ChangeTracker.Entries().Where(e =>
                           (e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added)
                              && (IncludeEntity(e) || IncludeEntityName(e, FullName))).ToList();
            }
            else
            {
                entries = context.ChangeTracker.Entries().Where(e =>
                            (e.State == EntityState.Deleted || e.State == EntityState.Modified)
                            && (IncludeEntity(e) || IncludeEntityName(e, FullName))).ToList();
            }
            if (entries.Count == 0)
            {
                return;
            }
            if (autoHistoryConfig.OperatorIsLogin)
            {
                if (Operator == "SyStem")
                {
                    var userinfo = ClaimsIdentityServer.AccountUserid();
                    if (userinfo.UserId > 0) { Operator = userinfo.UserNick + "|" + userinfo.UserId; }
                }
            }
            foreach (var entry in entries)
            {
                var history = new AutoHistory
                {
                    TypeName = entry.ShowTypeName(),
                    Operator = Operator,
                    Kind = entry.State,
                };
                history.Ip = HttpContext.GetIp();
                switch (entry.State)
                {
                    case EntityState.Added:
                        // REVIEW: what's the best way to do this?
                        history.SourceId = "0";
                        history.Kind = EntityState.Added;
                        history.AfterJson = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, jsonSetting);
                        break;
                    case EntityState.Deleted:
                        history.SourceId = entry.PrimaryKey();
                        history.Kind = EntityState.Deleted;
                        history.BeforeJson = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, jsonSetting);
                        break;
                    case EntityState.Modified:
                        history.SourceId = entry.PrimaryKey();
                        history.Kind = EntityState.Modified;
                        history.BeforeJson = JsonConvert.SerializeObject(entry.Original(), Formatting.Indented, jsonSetting);
                        history.AfterJson = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, jsonSetting);
                        break;
                    default:
                        continue;
                }
                context.AddAsync(history);
            }
        }

        private static object Original(this EntityEntry entry)
        {
            var type = entry.Entity.GetType();
            var typeInfo = type.GetTypeInfo();

            // Create a entity instance.
            var entity = Activator.CreateInstance(type, true);

            // Get the mapped properties for the entity type from EF metadata.
            // (include shadow properties, not include navigations)
            var properties = entry.Metadata.GetProperties();
            foreach (var property in properties)
            {
                // TODO: Supports the shadow properties
                if (property.IsShadowProperty)
                {
                    continue;
                }

                var entityProperty = typeInfo.GetProperty(property.Name);

                // Get the original value
                var original = entry.Property(property.Name).OriginalValue;

                // Set the original value to entity property.
                entityProperty.SetValue(entity, original);
            }

            return entity;
        }

        private static string PrimaryKey(this EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();

            var values = new List<object>();
            foreach (var property in key.Properties)
            {
                var value = entry.Property(property.Name).CurrentValue;
                if (value != null)
                {
                    values.Add(value);
                }
            }

            return string.Join(",", values);
        }

        private static bool IncludeEntity(EntityEntry entry)
        {
            var type = entry.Entity.GetType();
            bool? result = EnsureEntitiesIncludeIgnoreAttrCache(type); //true:excluded false=ignored null=unknown
            // Include all, except the explicitly ignored entities
            return result == null || result.Value;
        }

        private static bool IncludeEntityName(EntityEntry entry, string FullName)
        {
            if (string.IsNullOrEmpty(FullName)) return false;

            if (entry.Entity.GetType().FullName == FullName) return true;

            return false;

        }

        private static bool? EnsureEntitiesIncludeIgnoreAttrCache(Type type)
        {
            if (!EntitiesIncludeIgnoreAttrCache.ContainsKey(type))
            {
                var includeAttr = type.GetTypeInfo().GetCustomAttribute(typeof(AuditIncludeAttribute));
                if (includeAttr != null)
                {
                    EntitiesIncludeIgnoreAttrCache[type] = true; // Type Included by IncludeAttribute
                }
                else if (type.GetTypeInfo().GetCustomAttribute(typeof(AuditIgnoreAttribute)) != null)
                {
                    EntitiesIncludeIgnoreAttrCache[type] = false; // Type Ignored by IgnoreAttribute
                }
                else
                {
                    EntitiesIncludeIgnoreAttrCache[type] = autoHistoryConfig.IsAutoHistory; // No attribute
                }
            }
            return EntitiesIncludeIgnoreAttrCache[type];
        }

        private static AutoHistoryConfig ReadConfig(string file = "config/AutoHistory.json")
        {
            var config = new ConfigHelper();
            var entityconfig = new AutoHistoryConfig();
            try
            {
                entityconfig = config.Reader<AutoHistoryConfig>(file);

            }
            catch (Exception e)
            {
                entityconfig = new AutoHistoryConfig()
                {
                    IsHistory = false,
                };

                LogHelper.LogException(e);
            }
            return entityconfig;
        }

        private static string ShowTypeName(this EntityEntry entry)
        {
            var result = entry.Entity.GetType().Name;
            if (autoHistoryConfig.TypeName != "Name") {
                result = entry.Entity.GetType().FullName;
            }
            return result;
        }

    }
}
