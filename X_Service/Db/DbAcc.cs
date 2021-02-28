using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace X_Service.Db {
    public class DbAcc {
        //<appSettings>
        //    <add key="DataBase" value="Data/oData.mdb"/>
        //</appSettings>

        //protected static string connectionString = "Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["OledbConn"]);
        protected static string connectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + Directory.GetCurrentDirectory() + ( @"\xkusers.mdb" );//用于指定数据库文件与.exe文件在同一目录下;

        #region  ACCESS数据库基础类
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <returns>INT</returns>
        public static int ExecuteSql ( string SQLString ) {
            using ( OleDbConnection connection = new OleDbConnection(connectionString) ) {
                using ( OleDbCommand cmd = new OleDbCommand(SQLString , connection) ) {
                    try {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    } catch ( System.Data.OleDb.OleDbException E ) {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL，带参数
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <param name="content">内容</param>
        /// <returns>INT</returns>
        public static int ExecuteSql ( string SQLString , string content ) {
            using ( OleDbConnection connection = new OleDbConnection(connectionString) ) {
                OleDbCommand cmd = new OleDbCommand(SQLString , connection);
                System.Data.OleDb.OleDbParameter myParameter = new System.Data.OleDb.OleDbParameter("@content" , OleDbType.VarWChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                } catch ( System.Data.OleDb.OleDbException E ) {
                    throw new Exception(E.Message);
                } finally {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行SQL数组
        /// </summary>
        /// <param name="SQLStringList">SQL数组</param>
        public static void ExecuteSqlTran ( ArrayList SQLStringList ) {
            using ( OleDbConnection conn = new OleDbConnection(connectionString) ) {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try {
                    for ( int n = 0; n < SQLStringList.Count; n++ ) {
                        string strsql = SQLStringList[n].ToString();
                        if ( strsql.Trim().Length > 1 ) {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                } catch ( System.Data.OleDb.OleDbException E ) {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }


        /// <summary>
        /// DataTable
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable ( string SQLString ) {
            using ( OleDbConnection connection = new OleDbConnection(connectionString) ) {
                DataSet ds = new DataSet();
                try {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString , connection);
                    command.Fill(ds);
                } catch ( System.Data.OleDb.OleDbException ex ) {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 获得DataTable
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable ( string SQLString , int StartIndex , int PageSize ) {
            using ( OleDbConnection connection = new OleDbConnection(connectionString) ) {
                DataSet ds = new DataSet();
                try {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString , connection);
                    command.Fill(ds , StartIndex , PageSize , "Tb");
                } catch ( System.Data.OleDb.OleDbException ex ) {
                    throw new Exception(ex.Message);
                }
                return ds.Tables["Tb"];
            }
        }

        /// <summary>
        /// 获得IDataReader
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetReader ( string strSQL ) {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(strSQL , connection);
            try {
                connection.Open();
                OleDbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;

            } catch ( System.Data.OleDb.OleDbException e ) {
                throw new Exception(e.Message);
            }
        }



        /// <summary>
        /// 获得单一字段
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>返回object</returns>
        public static object GetScalar ( string SQLString ) {
            using ( OleDbConnection connection = new OleDbConnection(connectionString) ) {
                using ( OleDbCommand cmd = new OleDbCommand(SQLString , connection) ) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ( ( Object.Equals(obj , null) ) || ( Object.Equals(obj , System.DBNull.Value) ) ) {
                            return null;
                        } else {
                            return obj;
                        }
                    } catch ( System.Data.OleDb.OleDbException e ) {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public static int ExecuteInsert ( DataTable dt ) {

            string sql = "";
            string fieldStr = "";
            string valueStr = "";
            int i = 0;
            try {
                for ( i = 0; i < dt.Columns.Count; i++ ) {
                    fieldStr += dt.Columns[i].ColumnName + ",";
                }

                fieldStr = fieldStr.Substring(0 , fieldStr.Length - 1);

                for ( i = 0; i < dt.Rows.Count; i++ ) {
                    sql = "insert into {0}({1}) values({2})";
                    valueStr = "";
                    for ( int j = 0; j < dt.Columns.Count; j++ ) {

                        switch ( System.Type.GetTypeCode(dt.Rows[i][j].GetType()) ) {
                            case System.TypeCode.Byte:
                            case System.TypeCode.Char:
                            case System.TypeCode.Decimal:
                            case System.TypeCode.Double:
                            case System.TypeCode.Int16:
                            case System.TypeCode.Int32:
                            case System.TypeCode.Int64:
                            case System.TypeCode.SByte:
                            case System.TypeCode.Single:
                            case System.TypeCode.UInt16:
                            case System.TypeCode.UInt32:
                            case System.TypeCode.UInt64:
                                valueStr += dt.Rows[i][j].ToString() + ",";
                                break;
                            case System.TypeCode.DateTime:
                            case System.TypeCode.String:
                                valueStr += "'" + dt.Rows[i][j].ToString() + "',";
                                break;
                            case System.TypeCode.Boolean: {
                                    if ( (bool)dt.Rows[i][j] == true )
                                        valueStr += "1,";
                                    else
                                        valueStr += "0,";

                                    break;
                                }
                            default:
                                valueStr += " null,";
                                break;
                        }
                    }

                    valueStr = valueStr.Substring(0 , valueStr.Length - 1);
                    sql = string.Format(sql , dt.TableName , fieldStr , valueStr);

                    try {
                        ExecuteSql(sql);
                    } catch {
                        continue;
                    }

                }
                return 1;
            } catch {
                return 0;

            }
        }
        #endregion
    }


}
