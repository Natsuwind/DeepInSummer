//===============================================================================
//Author:Caicaihui
//E-mail:Caicaihui@126.com
//WebSite:www.36510.com
//作者申明：可以被用于商业目的，唯一的条件是保留组件中的版权信息
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace Yuwen.Tools.Data
{
    /// <summary>
    /// DBHelper 的摘要说明
    /// </summary>
    public class DBHelper
    {
        private DbConnection MyConnection;
        private DbCommand MyCommand;
        private DbDataAdapter MyDataAdapter;
        private DbProviderFactory MyFactory;

                #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBHelper(): this("DataLink")
        {
        }

        /// <summary>
        /// 构造函数--重载方法一
        /// </summary>
        /// <param name="datalink">connectionStrings节点处name值</param>
        public DBHelper(string datalink): this(ConfigurationManager.ConnectionStrings[datalink].ConnectionString, ConfigurationManager.ConnectionStrings[datalink].ProviderName)
        {
        }

        /// <summary>
        /// 构造函数--重载方法二
        /// </summary>
        /// <param name="connectionstring">数据库链接</param>
        /// <param name="databasetype">数据库的类型</param>
        public DBHelper(string connectionstring, string databasetype)
        {
            MyFactory = DbProviderFactories.GetFactory(databasetype);
            MyConnection = MyFactory.CreateConnection();
            MyConnection.ConnectionString = (databasetype.ToString() == "System.Data.OleDb") ? ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GetDataPath(connectionstring) + ";") : (connectionstring);
            MyCommand = MyConnection.CreateCommand();
        }
        #endregion

        #region 返回DataSet
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql, 0, 0, "Table");
        }

        /// <summary>
        /// 返回DataSet--重载方法一
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <param name="dataname">虚拟表名</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql, string dataname)
        {
            return ExecuteDataSet(sql, 0, 0, dataname);
        }

        /// <summary>
        /// 返回DataSet--重载方法二
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <param name="startindex">开始行</param>
        /// <param name="num">总行数</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql, int startindex, int num)
        {
            return ExecuteDataSet(sql, startindex, num, "Table");
        }

        /// <summary>
        /// 返回DataSet--重载方法三
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <param name="startindex">开始行</param>
        /// <param name="num">总行数</param>
        /// <param name="dataname">虚拟表名</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql, int startindex, int num, string dataname)
        {
            DataSet MyDataSet = new DataSet();
            IsStoredProcedure(sql);
            MyDataAdapter = MyFactory.CreateDataAdapter();
            MyDataAdapter.SelectCommand = MyCommand;
            try
            {
                Open();
                MyDataAdapter.Fill(MyDataSet, startindex, num, dataname);
                MyDataAdapter.Dispose();
                return MyDataSet;
            }
            finally
            {
                Dispose();
            }
        }
        #endregion

        #region 返回DataReader
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>void</returns>
        public DbDataReader ExecuteReader(string sql)
        {
            IsStoredProcedure(sql);
            try
            {
                Open();
                return MyCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                Dispose();
                return null;
            }
        }
        #endregion

        #region 插入、删除、更新数据
        /// <summary>
        /// 插入、删除、更新数据
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>object</returns>
        public int ExecuteNonQuery(string sql)
        {
            IsStoredProcedure(sql);
            try
            {
                Open();
                return MyCommand.ExecuteNonQuery();
            }
            finally
            {
                Dispose();
            }
        }
        #endregion

        #region 统计数据
        /// <summary>
        /// 统计数据
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string sql)
        {
            IsStoredProcedure(sql);
            try
            {
                Open();
                return MyCommand.ExecuteScalar();
            }
            finally
            {
                Dispose();
            }
        }
        #endregion

        #region 判断是否为存储过程
        /// <summary>
        /// 判断是否为存储过程
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>void</returns>
        internal void IsStoredProcedure(string sql)
        {
            MyCommand.CommandText = sql;
            if (!sql.Contains(" "))
            {
                MyCommand.CommandType = CommandType.StoredProcedure;
            }
        }
        #endregion

        #region 数据库链接的相对路径和绝对路径处理
        /// <summary>
        /// 数据库链接的相对路径和绝对路径处理
        /// </summary>
        /// <param name="datalink">数据库的name值</param>
        /// <returns>void</returns>
        internal string GetDataPath(string datalink)
        {
            if (!datalink.Contains(":"))
            {
                return System.Web.HttpContext.Current.Server.MapPath(datalink);
            }
            return datalink;
        }
        #endregion

        #region 存储过程的参数部分--输出、输入、返回
        /// <summary>
        /// 参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值</param>
        /// <param name="type">参数值的类型</param>
        /// <param name="size">参数值的大小</param>
        /// <param name="Direction">参数的返回类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, object value, DbType type, int size, ParameterDirection Direction)
        {
            DbParameter MyDbParameter = MyCommand.CreateParameter();
            MyDbParameter.ParameterName = parameterName;
            MyDbParameter.Value = value;
            MyDbParameter.DbType = type;
            if (size != 0) { MyDbParameter.Size = size; }
            MyDbParameter.Direction = Direction;
            MyCommand.Parameters.Add(MyDbParameter);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值</param>
        /// <param name="type">参数值的类型</param>
        /// <param name="size">参数值的大小</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, object value, DbType type, int size)
        {
            ParameterAdd(parameterName, value, type, size, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,object类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, object value)
        {
            ParameterAdd(parameterName, value, DbType.Object, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,string类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, string value)
        {
            ParameterAdd(parameterName, value, DbType.String, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Int32类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Int32 value)
        {
            ParameterAdd(parameterName, value, DbType.Int32, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Int16类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Int16 value)
        {
            ParameterAdd(parameterName, value, DbType.Int16, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Boolean类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Boolean value)
        {
            ParameterAdd(parameterName, value, DbType.Boolean, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,UInt32类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, UInt32 value)
        {
            ParameterAdd(parameterName, value, DbType.UInt32, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,UInt16类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, UInt16 value)
        {
            ParameterAdd(parameterName, value, DbType.UInt16, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Byte类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Byte value)
        {
            ParameterAdd(parameterName, value, DbType.Byte, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Decimal类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Decimal value)
        {
            ParameterAdd(parameterName, value, DbType.Decimal, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Double类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Double value)
        {
            ParameterAdd(parameterName, value, DbType.Double, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,DateTime类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, DateTime value)
        {
            ParameterAdd(parameterName, value, DbType.DateTime, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输入的参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="value">参数的值,Single类型</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Single value)
        {
            ParameterAdd(parameterName, value, DbType.Single, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// 带输出参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <param name="type">参数值的类型</param>
        /// <param name="size">参数值的大小</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, DbType type, int size)
        {
            ParameterAdd(parameterName, null, type, size, ParameterDirection.Output);
        }

        /// <summary>
        /// 带输出参数组
        /// </summary>
        /// <param name="parameterName">参数的名称</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName)
        {
            ParameterAdd(parameterName, null, DbType.String, 255, ParameterDirection.Output);
        }

        /// <summary>
        /// 带返回参数组
        /// </summary>
        /// <returns>void</returns>
        public void ParameterAdd()
        {
            ParameterAdd("@return", null, DbType.String, 255, ParameterDirection.ReturnValue);
        }
        #endregion

        #region 存储过程的参数部分--取参数的返回值
        /// <summary>
        /// 按序号返回参数值，一般在执行完存储过程后使用
        /// </summary>
        /// <param name="ParameterIndex">序号</param>
        /// <returns>返回参数的内容</returns>
        public string this[int ParameterIndex]
        {
            get
            {
                return MyCommand.Parameters[ParameterIndex].Value.ToString();
            }
        }

        /// <summary>
        /// 按名称返回参数值，一般在执行完存储过程后使用
        /// </summary>
        /// <param name="ParameterName">参数名称。比如 @UserName</param>
        /// <returns>返回参数的内容</returns>
        public string this[string ParameterName]
        {
            get
            {
                return MyCommand.Parameters[ParameterName].Value.ToString();
            }
        }
        #endregion

        #region 打开数据库链接和释放资源
        /// <summary>
        /// 打开数据库链接
        /// </summary>
        internal void Open()
        {
            if (MyConnection.State == ConnectionState.Closed)
            {
                MyConnection.Open();
            }
        }

        /// <summary>
        /// 释放Connection资源
        /// </summary>
        internal void Dispose()
        {
            if (MyConnection.State == ConnectionState.Open)
            {
                MyConnection.Close();
                //MyConnection.Dispose();
                //MyConnection = null;
            }
        }

        /// <summary>
        /// 释放Parameters资源,一般在执行完存储过程后使用 
        /// </summary>
        /// <remarks>
        /// 凡是使用了ParameterAdd方法，作者强烈建议使用ParametersClear()方法来释放资源
        /// </remarks>
        public void ParametersClear()
        {
            if (MyCommand.Parameters != null)
            {
                MyCommand.Parameters.Clear();
            }
            if (MyCommand != null)
            {
                MyCommand.Dispose();
            }
        }
        #endregion
    }
}