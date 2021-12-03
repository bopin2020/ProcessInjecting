using System;
using System.Collections.Generic;

namespace Models
{
    public interface IService
    {
        Metadata Injectionmetadata { get; set; }

        bool Init();

        /// <summary>
        /// 只包含一组 键值对时  使用KeyValuePair
        /// </summary>
        List<Dictionary<ProcessKind, Object[]>> paras { get; set; }

    }
}
