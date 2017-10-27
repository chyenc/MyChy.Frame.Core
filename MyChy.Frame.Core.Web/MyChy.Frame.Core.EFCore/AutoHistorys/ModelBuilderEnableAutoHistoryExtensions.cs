﻿using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.AutoHistorys.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.AutoHistorys
{
    /// <summary>
    /// Represents a plugin for Microsoft.EntityFrameworkCore to support automatically recording data changes history.
    /// </summary>
    public static class ModelBuilderEnableAutoHistoryExtensions
    {
        /// <summary>
        /// Enables the automatic recording change history.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to enable auto history functionality.</param>
        /// <returns>The <see cref="ModelBuilder"/> to enable auto history functionality.</returns>
        public static ModelBuilder EnableAutoHistory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutoHistory>(b => {
                b.HasKey(x => x.Id);
                b.Property(c => c.SourceId).IsRequired().HasMaxLength(50);
                b.Property(c => c.Operator).IsRequired().HasMaxLength(50);
                b.Property(c => c.TypeName).IsRequired().HasMaxLength(128);
                //b.Property(c => c.BeforeJson).HasMaxLength(2048);
                //b.Property(c => c.AfterJson).HasMaxLength(2048);
                // Shadow properties
                //b.Property<DateTime>("LastUpdated");
                // This MSSQL only
                //b.Property(c => c.CreateTime).HasDefaultValueSql("getdate()");
                b.ToTable("Core_AutoHistory");
            });

            return modelBuilder;
        }
    }
}
