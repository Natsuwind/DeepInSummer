using System;
using System.Data;
using System.Data.Common;

namespace Natsuhime.TinyData
{
    public class DBHelper
    {
        private DbProviderFactory dbProviderFactory;
        private DbConnection dbConnection;
        private DbCommand dbCommand;
        private DbDataAdapter dbDataAdapter;

        /// <summary>
        /// 查询次数统计
        /// </summary>
        private int m_querycount = 0;

        public int Querycount
        {
            get { return m_querycount; }
            set { m_querycount = value; }
        }

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnectString">链接字符串</param>
        /// <param name="dbProviderNamespace">数据提供者的命名空间,例如:"System.Data.SqlClient"</param>
        public DBHelper(string dbConnectString, string dbProviderNamespace)
        {
            dbProviderFactory = DbProviderFactories.GetFactory(dbProviderNamespace);
            dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = dbConnectString;
            dbCommand = dbConnection.CreateCommand();
        }
        /// <summary>
        /// 构造函数重载(sqlclient)
        /// </summary>
        /// <param name="dbConnectString">链接字符串</param>
        public DBHelper(string dbConnectString)
            : this(dbConnectString, "System.Data.SqlClient") { }
        #endregion

        #region 打开和关闭数据库连接
        public void Open()
        {
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }
        public void Close()
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }
        /// <summary>
        /// 兼容以前版本,其实就是close()方法
        /// </summary>
        public void Dispose()
        {
            Close();
            dbConnection.Dispose();
        }
        #endregion

        #region 执行方法

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
            DataSet ds = new DataSet();
            IsStoredProcedure(sql);
            dbDataAdapter = dbProviderFactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = dbCommand;

            dbDataAdapter.Fill(ds, startindex, num, dataname);
            dbDataAdapter.Dispose();
            m_querycount++;
            return ds;
        }
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>void</returns>
        public DbDataReader ExecuteReader(string sql)
        {
            IsStoredProcedure(sql);            
            m_querycount++;
            return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

        }


        /// <summary>
        /// 插入、删除、更新数据
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>object</returns>
        public int ExecuteNonQuery(string sql)
        {
            IsStoredProcedure(sql);
            m_querycount++;
            return dbCommand.ExecuteNonQuery();

        }
        /// <summary>
        /// 统计数据
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string sql)
        {
            IsStoredProcedure(sql);            
            m_querycount++;
            return dbCommand.ExecuteScalar();
        }
        /// <summary>
        /// 判断是否为存储过程
        /// </summary>
        /// <param name="sql">SQL语句或者存储过程</param>
        /// <returns>void</returns>
        internal void IsStoredProcedure(string sql)
        {
            dbCommand.CommandText = sql;
            if (!sql.Contains(" "))
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
            }
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
            DbParameter dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.DbType = type;
            if (size != 0) { dbParameter.Size = size; }
            dbParameter.Direction = Direction;
            dbCommand.Parameters.Add(dbParameter);
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

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,string类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, string value)
        //{
        //    ParameterAdd(parameterName, value, DbType.String, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Int32类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Int32 value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Int32, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Int16类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Int16 value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Int16, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Boolean类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Boolean value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Boolean, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,UInt32类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, UInt32 value)
        //{
        //    ParameterAdd(parameterName, value, DbType.UInt32, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,UInt16类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, UInt16 value)
        //{
        //    ParameterAdd(parameterName, value, DbType.UInt16, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Byte类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Byte value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Byte, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Decimal类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Decimal value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Decimal, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Double类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Double value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Double, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,DateTime类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, DateTime value)
        //{
        //    ParameterAdd(parameterName, value, DbType.DateTime, 0, ParameterDirection.Input);
        //}

        ///// <summary>
        ///// 带输入的参数组
        ///// </summary>
        ///// <param name="parameterName">参数的名称</param>
        ///// <param name="value">参数的值,Single类型</param>
        ///// <returns>void</returns>
        //public void ParameterAdd(string parameterName, Single value)
        //{
        //    ParameterAdd(parameterName, value, DbType.Single, 0, ParameterDirection.Input);
        //}

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
                return dbCommand.Parameters[ParameterIndex].Value.ToString();
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
                return dbCommand.Parameters[ParameterName].Value.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 清理和释放Parameters资源
        /// </summary>
        /// <remarks>
        /// 凡是使用了ParameterAdd方法，作者强烈建议使用ParametersClear()方法来释放资源
        /// </remarks>
        public void ParametersClear()
        {
            if (dbCommand.Parameters != null)
            {
                dbCommand.Parameters.Clear();
            }
            if (dbCommand != null)
            {
                dbCommand.Dispose();
            }
        }


        #region 数据库小工具
        /// <summary>
        /// 修改自增字段seed,目前只支持MSSQL
        /// </summary>
        public void ChangeIdentity(string TableName, string FieldName, int Seed)
        {
            if (TableName == string.Empty)
                throw new Exception("要修改自增字段的数据库表名不能为空!");
            ExecuteNonQuery(string.Format(@"DBCC CHECKIDENT ('{0}', RESEED, {1});", TableName, Seed - 1));
        }
        /// <summary>
        /// 兼容以前版本.新版为ChangeIdentity()
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <param name="Seed"></param>
        public void ChangeIdEntity(string TableName, string FieldName, int Seed)
        {
            ChangeIdentity(TableName, FieldName, Seed);
        }
        /// <summary>
        /// 自增插入开启
        /// </summary>
        /// <param name="TableName"></param>
        public void SetIdentityInsertON(string TableName)
        {
            if (TableName == string.Empty)
                throw new Exception("表名不能为空!");
            ExecuteNonQuery(string.Format("SET IDENTITY_INSERT {0} ON", TableName));
        }
        /// <summary>
        /// 自增插入关闭
        /// </summary>
        /// <param name="TableName"></param>
        public void SetIdentityInsertOFF(string TableName)
        {
            if (TableName == string.Empty)
                throw new Exception("表名不能为空!");
            ExecuteNonQuery(string.Format("SET IDENTITY_INSERT {0} OFF", TableName));
        }
        /// <summary>
        /// 清空表
        /// </summary>
        /// <param name="TableName"></param>
        public void TruncateTable(string TableName)
        {
            if (TableName == string.Empty)
                throw new Exception("表名不能为空!");
            ExecuteNonQuery(string.Format("TRUNCATE TABLE {0}", TableName));
        }

        #endregion
    }
}
