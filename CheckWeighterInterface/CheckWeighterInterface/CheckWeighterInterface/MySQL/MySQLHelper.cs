using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraSplashScreen;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CheckWeighterInterface.MySQL
{
    class MySQLHelper
    {
        public MySqlConnection conn;   //连接对象
        private string DataSource = String.Empty;
        private string DbName = String.Empty;
        private string UserName = String.Empty;
        private string Password = String.Empty;
        private string connStr = String.Empty;

        public MySQLHelper(string host, string db, string user, string pw)
        {
            this.DataSource = host;
            this.DbName = db;
            this.UserName = user;
            this.Password = pw;
            connStr = "data source=" + DataSource + ";database=" + DbName + ";user id=" + UserName + ";password=" + Password + ";";

            //SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, Program.progressPercentVal+=5);
        }

        //mysql连接
        public bool _connectMySQL()
        {
            bool flag = false;
            try
            {
                this.conn = new MySqlConnection(this.connStr);
                this.conn.Open();
                flag = true;
            }
            catch (SystemException ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }

        //mysql查询，填充DataTable
        public bool _queryTableMySQL(string queryCmd, ref DataTable resultDt)
        {
            bool flag = false;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(queryCmd, this.conn);//创建MySqlCommand对象
                myCommand.CommandTimeout = 12000;//超时
                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();//开启连接
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(queryCmd, this.conn);//创建MySqlDataAdapter对象
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);//此处必须有，否则无法更新
                resultDt.Rows.Clear();  //填充前清空表
                adapter.Fill(resultDt);//将查询的内容填充到数据表中
                flag = true;
            }
            catch (SystemException ex)
            {
                ex.ToString();
            }
            return flag;
        }

        //mysql插入记录，直接将数据和指令拼接
        public bool _insertMySQL(string insertCmd)
        {
            bool flag = false;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(insertCmd, this.conn);//创建MySqlCommand对象
                myCommand.CommandType = CommandType.Text;//命令类型
                myCommand.CommandTimeout = 12000;

                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();//开启连接
                }
                if (myCommand.ExecuteNonQuery() > 0)
                {
                    flag = true;
                }//更新内容(返回受影响函数，如增、删、改操作)
            }
            catch (SystemException ex)
            {
                ex.ToString();
            }
            return flag;
        }

        //mysql插入记录，将数据封装成MySqlParameter变量，防参数化注入
        public void _InsertMySQLUseParameter(String sqlInsert, MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand myCommand = new MySqlCommand(sqlInsert, this.conn);//创建MySqlCommand对象
                myCommand.CommandType = CommandType.Text;//命令类型
                myCommand.CommandTimeout = 12000;
                if (parameters != null)//添加参数
                {
                    foreach (MySqlParameter parm in parameters)
                        myCommand.Parameters.Add(parm);
                }

                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();//开启连接
                }
                myCommand.ExecuteNonQuery();//更新内容(返回受影响函数，如增、删、改操作)
            }
            catch (SystemException ex)
            {
                ex.ToString();
            }
        }

        //mysql删除记录
        public bool _deleteMySQL(string delCmd)
        {
            bool flag = false;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(delCmd, this.conn);//创建MySqlCommand对象
                myCommand.CommandType = CommandType.Text;//命令类型
                myCommand.CommandTimeout = 12000;

                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();//开启连接
                }
                if (myCommand.ExecuteNonQuery() > 0)
                {
                    flag = true;
                }//更新内容(返回受影响函数，如增、删、改操作)
            }
            catch (SystemException ex)
            {
                ex.ToString();
            }
            return flag;
        }

        public bool _updateMySQL(string updateCmd)
        {
            bool flag = false;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(updateCmd, this.conn);//创建MySqlCommand对象
                myCommand.CommandType = CommandType.Text;//命令类型
                myCommand.CommandTimeout = 12000;

                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();//开启连接
                }
                if (myCommand.ExecuteNonQuery() > 0)
                {
                    flag = true;
                }//更新内容(返回受影响函数，如增、删、改操作)
            }
            catch (SystemException ex)
            {
                ex.ToString();
            }
            return flag;
        }

        //参数列表：存储过程名、封装的参数、输入参数个数、输出参数个数
        public bool _executeProcMySQL(string cmdExecute, MySqlParameter[] parameters, int inParaCount, int outParaCount)
        {
            bool flag = false;
            try
            {
                MySqlCommand myCommand = new MySqlCommand(cmdExecute, conn);//创建MySqlCommand对象
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 12000;
                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();//开启连接
                }
                for (int i = 0; i < inParaCount; i++)
                {
                    parameters[i].Direction = ParameterDirection.Input;
                    myCommand.Parameters.Add(parameters[i]);
                }

                for (int i = 0; i < outParaCount; i++)
                {
                    parameters[inParaCount + i].Direction = ParameterDirection.Output;
                    myCommand.Parameters.Add(parameters[inParaCount + i]);
                }

                int rows = myCommand.ExecuteNonQuery();
                if (rows > 0)
                {
                    flag = true;
                }
                myCommand.Parameters.Clear();
            }
            catch (SystemException ex)
            {
                ex.ToString();
            }
            return flag;
        }



    }
}
