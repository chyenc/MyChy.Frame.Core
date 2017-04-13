﻿using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.Common.Core;
using MyChy.Frame.Core.EFCore.Abstraction;
using MyChy.Frame.Core.EFCore.AutoHistorys;
using MyChy.Frame.Core.EFCore.Entitys.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.EFCore
{
    public class CoreDbContext : DbContext
    {
        // private readonly IEFCacheServiceProvider _cacheServiceProvider;
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
            //  Database.EnsureCreated();
            //  _cacheServiceProvider = cacheServiceProvider;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Type> typeToRegisterCustomModelBuilders = new List<Type>();
            List<Type> typeToRegisterEntitys = new List<Type>();
            foreach (var assemblie in ExtensionManager.Assemblies)
            {
                //获取所有继承BaseEntity的实体
                var entityClassTypes = assemblie.ExportedTypes.Where(x =>
                //( x.GetTypeInfo().IsSubclassOf(typeof(BaseEntity)) && !x.GetTypeInfo().IsAbstract)||
                typeof(IEntity).IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract
                // && !x.GetTypeInfo().IsDefined(typeof(MapIgnoreAttribute), false)
                );

                typeToRegisterEntitys.AddRange(entityClassTypes);

                //获取所有继承ICustomModelBuilder的实体映射
                var customModelBuilderClassTypes = assemblie.ExportedTypes.Where(
                    x => typeof(ICustomModelBuilder).IsAssignableFrom(x) && x.GetTypeInfo().IsClass);
                typeToRegisterCustomModelBuilders.AddRange(customModelBuilderClassTypes);
            }

            //把实体注册到模型构建中
            RegisterEntities(modelBuilder, typeToRegisterEntitys);
            //构建所有继承ICustomModelBuilder的实体映射
            RegiserConvention(modelBuilder);

            base.OnModelCreating(modelBuilder);

            RegisterCustomMappings(modelBuilder, typeToRegisterCustomModelBuilders);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=192.168.1.101;Database=SF_Team_2017;uid=sa;pwd=123.com.cn;Pooling=True;Min Pool Size=1;Max Pool Size=100;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;",
            //   b => b.MigrationsAssembly("SF.WebHost"));
            base.OnConfiguring(optionsBuilder);
        }

        private static void RegiserConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.ClrType.Namespace != null)
                {
                    var nameParts = entity.ClrType.Namespace.Split('.');
                    var tableName = string.Concat(nameParts[1], "_", entity.ClrType.Name);
                    if (nameParts.Contains("Module"))
                        tableName = string.Concat(nameParts[2], "_", entity.ClrType.Name);
                    modelBuilder.Entity(entity.Name).ToTable(tableName);
                }
            }
        }

        private static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {

            //   var entityTypes = typeToRegisters.Where(x => (x.GetTypeInfo().IsSubclassOf(typeof(Entity))|| x.GetTypeInfo().IsSubclassOf(typeof(EntityWithTypedId<>))) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in typeToRegisters)
            {
                modelBuilder.Entity(type);
            }
        }

        private static void RegisterCustomMappings(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            //  var customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));
            foreach (var builderType in typeToRegisters)
            {
                if (builderType != null && builderType != typeof(ICustomModelBuilder))
                {
                    var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }


        public int SaveAutoHistoryChanges()
        {
            // ensure auto history
            this.EnsureAutoHistory();
            //var changedEntityNames = this.GetChangedEntityNames();
            var result = base.SaveChanges();
            //  _cacheServiceProvider.InvalidateCacheDependencies(changedEntityNames);
            return result;
        }

        //public override int SaveChanges()
        //{
        //    // ensure auto history
        //    this.EnsureAutoHistory();
        //    //var changedEntityNames = this.GetChangedEntityNames();

        //    var result = base.SaveChanges();
        //    //  _cacheServiceProvider.InvalidateCacheDependencies(changedEntityNames);

        //    return result;
        //}

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            return result;
        }


        public Task<int> SaveAutoHistoryChangesAsync(
            bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            // ensure auto history
            this.EnsureAutoHistory();
            //  var changedEntityNames = this.GetChangedEntityNames();

            var result =base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            //  _cacheServiceProvider.InvalidateCacheDependencies(changedEntityNames);
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = base.SaveChangesAsync(cancellationToken);
            return result;

        }
    }
}