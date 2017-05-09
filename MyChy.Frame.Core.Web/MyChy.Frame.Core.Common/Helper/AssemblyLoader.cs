﻿using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace MyChy.Frame.Core.Common.Helper
{
    public class AssemblyLoader : AssemblyLoadContext
    {
        private string folderPath;

        public AssemblyLoader(string folderPath)
        {
            this.folderPath = folderPath;
        }
        // Not exactly sure about this
        protected override Assembly Load(AssemblyName assemblyName)
        {
            //var deps = DependencyContext.Default;
            //var res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).ToList();
            //var assembly = Assembly.Load(new AssemblyName(res.First().Name));
            //return assembly;

            var deps = DependencyContext.Default;
            var res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).ToList();
            if (res.Count > 0)
            {
                return Assembly.Load(new AssemblyName(res.First().Name));
            }
            //else
            //{
            //    var apiApplicationFileInfo = new FileInfo($"{folderPath}{Path.DirectorySeparatorChar}{assemblyName.Name}.dll");
            //    if (File.Exists(apiApplicationFileInfo.FullName))
            //    {
            //        var asl = new AssemblyLoader(apiApplicationFileInfo.DirectoryName);
            //        return asl.LoadFromAssemblyPath(apiApplicationFileInfo.FullName);
            //    }
            //}
            return Assembly.Load(assemblyName);
        }


    }
}
