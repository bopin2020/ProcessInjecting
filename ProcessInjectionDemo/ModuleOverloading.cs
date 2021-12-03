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
    /// https://github.com/hasherezade/module_overloading'
    /// a more stealthy variant of "Dll hollowing"
    /// Reference or inpired from https://twitter.com/TheRealWover/status/1193284444687392768?s=20
    /// 手动Map a dll into memory using a file handle and NtCreateSection(SEC_IMAGE)
    /// It appears different from
    /// read it into memory normally
    /// 
    /// 因为被映射是作为image的，PE会自动展开到虚拟形式。正常读取仅作为  raw form;可以手动 remap成virtual form
    /// </summary>
    partial class ModuleOverloading
    {
    }
    partial class  ModuleOverloading
    {
    }
}
