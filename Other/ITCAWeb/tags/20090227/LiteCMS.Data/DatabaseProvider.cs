using System;
using System.Text;
using System.Reflection;
using Natsuhime.Data;

namespace LiteCMS.Data
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
                _instance = (IDataProvider)Activator.CreateInstance(Type.GetType(string.Format("LiteCMS.Data.{0}.DataProvider, LiteCMS.Data.{0}", DbConfigs.GetConfig().Dbtype), false, true));
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
