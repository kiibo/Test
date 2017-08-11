using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Zebra
{
    public class MysqlHelper
    {
        MySqlConnection con;

        ///利用Web.Config中的ConnStr字符串来连接数据库
        public MysqlHelper()
        {
            string sqlstring = "Database=citic;Data Source=192.168.182.128;User Id=root;Password=citic;pooling=false;CharSet=utf8;port=3306"; 
            con = new MySqlConnection(sqlstring);
        }
        /// 数据连接
        /// <param name="ip"></param>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        public MysqlHelper(string ip, string database, string username, string pwd)
        {
            string sql = "Server=" + ip + ";UserName=" + username + ";Password=" + pwd + ";Database=" + database + ";Port=3306;CharSet=gb2312;Allow Zero Datetime=true";
            con = new MySqlConnection(sql);
        }

        #region SQL语句操作
        /// 执行SQL语句
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Runsql(string sql)
        {
            int i = -1;
            try
            {
                MySqlCommand com = new MySqlCommand(sql, con);
                con.Open();
                i = com.ExecuteNonQuery();
            }
            catch(Exception ex)
            {}
            finally
            {
                con.Close();
            }
            return i;
        }
        /// 执行带参数SQL语句
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int Runsql(string sql, IDataParameter[] parms)
        {
            int i = -1;
            try
            {
                MySqlCommand com = new MySqlCommand(sql, con);
                foreach (MySqlParameter par in parms)
                {
                    com.Parameters.Add(par);
                }
                con.Open();
                i = com.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            return i;
        }

        /// 执行SQL语句获得DATATABLE
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDatabysql(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, con);
                da.Fill(ds);
            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }

        /// 执行带参数SQL语句获得DATATABLE
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public DataTable GetDatabysql(string sql, IDataParameter[] parms)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand com = new MySqlCommand(sql, con);
                foreach (MySqlParameter par in parms)
                {
                    com.Parameters.Add(par);
                }
                MySqlDataAdapter da = new MySqlDataAdapter(com);
                da.Fill(ds);
            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }
        #endregion

        #region 操作存储过程
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="prcname"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int RunPrc(string prcname, IDataParameter[] parms)
        {
            int i = -1;
            try
            {
                MySqlCommand com = new MySqlCommand(prcname, con);
                com.CommandType = CommandType.StoredProcedure;
                foreach (MySqlParameter par in parms)
                {
                    com.Parameters.Add(par);
                }
                con.Open();
                i = com.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            return i;
        }

        /// <summary>
        /// 执行存储过程获得数据集
        /// </summary>
        /// <param name="prcname"></param>
        /// <returns></returns>
        public DataTable GetDataByPrc(string prcname)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand com = new MySqlCommand(prcname, con);
                com.CommandType = CommandType.StoredProcedure;
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                return ds.Tables[0];
            }

            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }
        /// <summary>
        /// 执行存储过程获得数据集(带参数)
        /// </summary>
        /// <param name="prcname"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public DataTable GetDataByPrc(string prcname, IDataParameter[] parms)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                MySqlCommand com = new MySqlCommand(prcname, con);
                com.CommandType = CommandType.StoredProcedure;
                foreach (MySqlParameter par in parms)
                {
                    com.Parameters.Add(par);
                }
                MySqlDataAdapter da = new MySqlDataAdapter(com);
                da.Fill(ds);
            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }
        #endregion
    }
}
