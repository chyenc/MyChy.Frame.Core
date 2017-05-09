using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.StartupTask
{
    public interface IStartupTask
    {
        /// <summary>
        /// 获取此任务的优先级。较低的数字先运行。
        /// </summary>
        /// <value>
        /// The priority of this task.
        /// </value>
        int Priority { get; }

        /// <summary>
        /// Runs the startup task.
        /// </summary>
        Task Run();

    }
}
