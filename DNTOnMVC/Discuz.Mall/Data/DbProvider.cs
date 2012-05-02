using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Mall.Data
{
    /// <summary>
    /// 数据访问提供者
    /// </summary>
    public class DbProvider
    {
        private static readonly DataProvider _dp = new DataProvider();
        private DbProvider()
        { }

        public static DataProvider GetInstance()
        {
            return _dp;
        }

    }
}
