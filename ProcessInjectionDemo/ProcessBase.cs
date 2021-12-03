// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessInjectionDemo
{
    /// <summary>
    /// 抽象基类
    /// </summary>
    public abstract class ProcessBase
    {
        protected string ProcessTechniqueName;

        public abstract void Run();

        public virtual Task ExeTask()
        {
            return Task.CompletedTask;
        }
    }
}
