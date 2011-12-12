//===============================================================================
//Author:Caicaihui
//E-mail:Caicaihui@126.com
//WebSite:www.36510.com
//�������������Ա�������ҵĿ�ģ�Ψһ�������Ǳ�������еİ�Ȩ��Ϣ
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace Yuwen.Tools.Data
{
    /// <summary>
    /// DBHelper ��ժҪ˵��
    /// </summary>
    public class DBHelper
    {
        private DbConnection MyConnection;
        private DbCommand MyCommand;
        private DbDataAdapter MyDataAdapter;
        private DbProviderFactory MyFactory;

                #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public DBHelper(): this("DataLink")
        {
        }

        /// <summary>
        /// ���캯��--���ط���һ
        /// </summary>
        /// <param name="datalink">connectionStrings�ڵ㴦nameֵ</param>
        public DBHelper(string datalink): this(ConfigurationManager.ConnectionStrings[datalink].ConnectionString, ConfigurationManager.ConnectionStrings[datalink].ProviderName)
        {
        }

        /// <summary>
        /// ���캯��--���ط�����
        /// </summary>
        /// <param name="connectionstring">���ݿ�����</param>
        /// <param name="databasetype">���ݿ������</param>
        public DBHelper(string connectionstring, string databasetype)
        {
            MyFactory = DbProviderFactories.GetFactory(databasetype);
            MyConnection = MyFactory.CreateConnection();
            MyConnection.ConnectionString = (databasetype.ToString() == "System.Data.OleDb") ? ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GetDataPath(connectionstring) + ";") : (connectionstring);
            MyCommand = MyConnection.CreateCommand();
        }
        #endregion

        #region ����DataSet
        /// <summary>
        /// ����DataSet
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql, 0, 0, "Table");
        }

        /// <summary>
        /// ����DataSet--���ط���һ
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
        /// <param name="dataname">�������</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql, string dataname)
        {
            return ExecuteDataSet(sql, 0, 0, dataname);
        }

        /// <summary>
        /// ����DataSet--���ط�����
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
        /// <param name="startindex">��ʼ��</param>
        /// <param name="num">������</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql, int startindex, int num)
        {
            return ExecuteDataSet(sql, startindex, num, "Table");
        }

        /// <summary>
        /// ����DataSet--���ط�����
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
        /// <param name="startindex">��ʼ��</param>
        /// <param name="num">������</param>
        /// <param name="dataname">�������</param>
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

        #region ����DataReader
        /// <summary>
        /// ����DataReader
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
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

        #region ���롢ɾ������������
        /// <summary>
        /// ���롢ɾ������������
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
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

        #region ͳ������
        /// <summary>
        /// ͳ������
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
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

        #region �ж��Ƿ�Ϊ�洢����
        /// <summary>
        /// �ж��Ƿ�Ϊ�洢����
        /// </summary>
        /// <param name="sql">SQL�����ߴ洢����</param>
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

        #region ���ݿ����ӵ����·���;���·������
        /// <summary>
        /// ���ݿ����ӵ����·���;���·������
        /// </summary>
        /// <param name="datalink">���ݿ��nameֵ</param>
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

        #region �洢���̵Ĳ�������--��������롢����
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ</param>
        /// <param name="type">����ֵ������</param>
        /// <param name="size">����ֵ�Ĵ�С</param>
        /// <param name="Direction">�����ķ�������</param>
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
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ</param>
        /// <param name="type">����ֵ������</param>
        /// <param name="size">����ֵ�Ĵ�С</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, object value, DbType type, int size)
        {
            ParameterAdd(parameterName, value, type, size, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,object����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, object value)
        {
            ParameterAdd(parameterName, value, DbType.Object, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,string����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, string value)
        {
            ParameterAdd(parameterName, value, DbType.String, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Int32����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Int32 value)
        {
            ParameterAdd(parameterName, value, DbType.Int32, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Int16����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Int16 value)
        {
            ParameterAdd(parameterName, value, DbType.Int16, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Boolean����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Boolean value)
        {
            ParameterAdd(parameterName, value, DbType.Boolean, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,UInt32����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, UInt32 value)
        {
            ParameterAdd(parameterName, value, DbType.UInt32, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,UInt16����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, UInt16 value)
        {
            ParameterAdd(parameterName, value, DbType.UInt16, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Byte����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Byte value)
        {
            ParameterAdd(parameterName, value, DbType.Byte, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Decimal����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Decimal value)
        {
            ParameterAdd(parameterName, value, DbType.Decimal, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Double����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Double value)
        {
            ParameterAdd(parameterName, value, DbType.Double, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,DateTime����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, DateTime value)
        {
            ParameterAdd(parameterName, value, DbType.DateTime, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// ������Ĳ�����
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="value">������ֵ,Single����</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, Single value)
        {
            ParameterAdd(parameterName, value, DbType.Single, 0, ParameterDirection.Input);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <param name="type">����ֵ������</param>
        /// <param name="size">����ֵ�Ĵ�С</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName, DbType type, int size)
        {
            ParameterAdd(parameterName, null, type, size, ParameterDirection.Output);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="parameterName">����������</param>
        /// <returns>void</returns>
        public void ParameterAdd(string parameterName)
        {
            ParameterAdd(parameterName, null, DbType.String, 255, ParameterDirection.Output);
        }

        /// <summary>
        /// �����ز�����
        /// </summary>
        /// <returns>void</returns>
        public void ParameterAdd()
        {
            ParameterAdd("@return", null, DbType.String, 255, ParameterDirection.ReturnValue);
        }
        #endregion

        #region �洢���̵Ĳ�������--ȡ�����ķ���ֵ
        /// <summary>
        /// ����ŷ��ز���ֵ��һ����ִ����洢���̺�ʹ��
        /// </summary>
        /// <param name="ParameterIndex">���</param>
        /// <returns>���ز���������</returns>
        public string this[int ParameterIndex]
        {
            get
            {
                return MyCommand.Parameters[ParameterIndex].Value.ToString();
            }
        }

        /// <summary>
        /// �����Ʒ��ز���ֵ��һ����ִ����洢���̺�ʹ��
        /// </summary>
        /// <param name="ParameterName">�������ơ����� @UserName</param>
        /// <returns>���ز���������</returns>
        public string this[string ParameterName]
        {
            get
            {
                return MyCommand.Parameters[ParameterName].Value.ToString();
            }
        }
        #endregion

        #region �����ݿ����Ӻ��ͷ���Դ
        /// <summary>
        /// �����ݿ�����
        /// </summary>
        internal void Open()
        {
            if (MyConnection.State == ConnectionState.Closed)
            {
                MyConnection.Open();
            }
        }

        /// <summary>
        /// �ͷ�Connection��Դ
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
        /// �ͷ�Parameters��Դ,һ����ִ����洢���̺�ʹ�� 
        /// </summary>
        /// <remarks>
        /// ����ʹ����ParameterAdd����������ǿ�ҽ���ʹ��ParametersClear()�������ͷ���Դ
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