using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.Abstraction;
using MyChy.Frame.Core.EFCore.Maps.Fluent;
using MyChy.Frame.Core.Web.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web.ModelBuilders
{
    public class CustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            CompetencesBuild(modelBuilder);

           // BaseBuild(modelBuilder);

        }

        /// <summary>
        /// 创建 Competences 建造
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void CompetencesBuild(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompUser>(b =>
            {
                b.MapCreatedMeta().MapUpdatedMeta().MapDeletedMeta();
                b.ToTable("CompUser");
            });

            //modelBuilder.Entity<CompUserRole>(b =>
            //{
            //    b.ToTable("CompUserRole");
            //});


        }
    }
}
