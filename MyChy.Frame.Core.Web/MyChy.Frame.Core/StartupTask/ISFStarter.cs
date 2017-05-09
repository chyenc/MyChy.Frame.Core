using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.StartupTask
{
    public interface ISFStarter
    {
        /// <summary>
        /// Runs the application SFStarter extension with specified <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The SFStarter <see cref="Context"/> containing assemblies to scan.</param>
        void Run();
    }
}
