using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace SqliteLibrary
{
    /// <summary>
    /// SQLite数据库操作基类
    /// </summary>
    public class SqliteHelper
    {
        // Fields
        private SQLiteConnection connection;
        public static string connectionString;

        #region Methods

        /// <summary>
        /// Sqlite数据库连接，
        /// </summary>
        /// <param name="connString"></param>
        public SqliteHelper(string connString)
        {
            connectionString = string.Empty;
            connectionString = connString;
        }

        public SqliteHelper()
        {
        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        private void OpenConnection()
        {
            try
            {
                this.connection = new SQLiteConnection(connectionString);
                this.connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void CloseConnection()
        {
            try
            {
                if (this.connection.State != System.Data.ConnectionState.Closed)
                {
                    this.connection.Close();
                    this.connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQuery(string sql, CommandType type, params SQLiteParameter[] param)
        {
            int result = 0;
            try
            {
                this.OpenConnection();
                SQLiteCommand command = new SQLiteCommand(sql, this.connection);
                if (param != null)
                {
                    command.Parameters.AddRange(param);
                }
                command.CommandType = type;
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
            return result;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="outputParam"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string procName, string outputParam, params SQLiteParameter[] param)
        {
            int result = 0;
            try
            {
                this.OpenConnection();
                SQLiteCommand command = new SQLiteCommand(procName, this.connection);
                if (param != null)
                {
                    command.Parameters.AddRange(param);
                }
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters[outputParam].Direction = System.Data.ParameterDirection.Output;
                command.ExecuteNonQuery();
                result = (int)command.Parameters[outputParam].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
            return result;
        }
        
        public SQLiteDataReader ExecuteReader(string sql,CommandType type,params SQLiteParameter[] param)
        {
            SQLiteDataReader reader=null;
            try
            {
                this.OpenConnection();
                SQLiteCommand command=new SQLiteCommand(sql,this.connection);
                if(param!=null)
                {
                    command.Parameters.AddRange(param);
                }
                command.CommandType=type;
                reader=command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch(Exception ex)
            {
                this.CloseConnection();
                throw ex;
            }
            return reader;
        
        }

        public object ExecuteScalar(string sql, CommandType type, params SQLiteParameter[] param)
        {
            object obj = null;
            try
            {
                this.OpenConnection();
                SQLiteCommand command = new SQLiteCommand(sql, this.connection);
                if (param != null)
                {
                    command.Parameters.AddRange(param);
                }
                command.CommandType = type;
                obj = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
            return obj;
        }

        public bool ExecuteTransaction(params string[] sqlList)
        {
            SQLiteTransaction transaction = null;
            bool result;
            try
            {
                this.OpenConnection();
                transaction = this.connection.BeginTransaction();
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = this.connection,
                    Transaction = transaction
                };
                foreach (string str in sqlList)
                {
                    command.CommandText = str;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
            return result;


        }

        public DataSet GetDataSet(string sql, CommandType type, params SQLiteParameter[] param)
        {
            DataSet dataSet = new DataSet();
            try
            {
                SQLiteDataAdapter dapter = new SQLiteDataAdapter(sql, connectionString);
                if (param != null)
                {
                    dapter.SelectCommand.Parameters.AddRange(param);
                }
                dapter.SelectCommand.CommandType = type;
                dapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataSet;

        }
        #region 不带参查询/ GetDataSet
        /// <summary>
        /// 不带参查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            DataSet dataSet = new DataSet();
            try
            {
                SQLiteDataAdapter dapter = new SQLiteDataAdapter(sql, connectionString);
                dapter.SelectCommand.CommandType = System.Data.CommandType.Text;
                dapter.Fill(dataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataSet;

        }
        #endregion
        #region 带参查询/ GetDataTable
        /// <summary>
        /// 带参查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type">System.Data.CommandType.Text(文本命令)/StoredProcedure(存储过程名称)/TableDirect(表名称)</param>
        /// <param name="param">不带参数是为：null</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, CommandType type, params SQLiteParameter[] param)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SQLiteDataAdapter dapter = new SQLiteDataAdapter(sql, connectionString);
                if (param != null)
                {
                    dapter.SelectCommand.Parameters.AddRange(param);
                }
                dapter.SelectCommand.CommandType = type;
                dapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }
        #endregion

        #region 不带参查询/ GetDataTable
        /// <summary>
        /// 不带参查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SQLiteDataAdapter dapter = new SQLiteDataAdapter(sql, connectionString);
                dapter.SelectCommand.CommandType = CommandType.Text;
                dapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }
        #endregion

        #endregion

    }
}
