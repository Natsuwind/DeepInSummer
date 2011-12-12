using System;
using System.Text;
using System.Reflection;
using Yuwen.Tools.Data;

namespace iTCA.Yuwen.Data
{
	public class DatabaseProvider
	{
		private DatabaseProvider()
		{ }

		private static IDataProvider _instance = null;
		private static object lockHelper = new object();

		static DatabaseProvider()
		{
			GetProvider();
		}

		private static void GetProvider()
		{
			try
			{
                _instance = (IDataProvider)Activator.CreateInstance(Type.GetType(string.Format("iTCA.Yuwen.Data.{0}.DataProvider, iTCA.Yuwen.Data.{0}", DBConfigs.GetDbType()), false, true));
			}
			catch
			{
				throw new Exception("请检查Dbtype节点数据库类型是否正确，例如：SqlServer、Access、MySql");
			}
		}

		public static IDataProvider GetInstance()
		{
			if (_instance == null)
			{
				lock (lockHelper)
				{
					if (_instance == null)
					{
						GetProvider();
					}
				}
			}
			return _instance;
		}

        public static void ResetDbProvider()
        {
            _instance = null; 
        }
	}
}
