using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace X_Service.Db {
    public class DbSqlserver {
        protected static string connectionString = "server=223.4.21.206;uid=tfr373767;pwd=GNEwMsAS;database=tfr373767";

        #region  Sql数据库基础类
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="SQLString">SQL</param>
        /// <returns>INT</returns>
        public static int ExecuteSql(string SQLString) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection)) {
                    try {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    } catch (System.Data.SqlClient.SqlException E) {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        public static int ExecuteSql(string SQLString, string content) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                } catch (System.Data.SqlClient.SqlException E) {
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
        public static void ExecuteSqlTran(ArrayList SQLStringList) {
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try {
                    for (int n = 0; n < SQLStringList.Count; n++) {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1) {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                } catch (System.Data.SqlClient.SqlException E) {
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
        public static DataTable GetDataTable(string SQLString) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                DataSet ds = new DataSet();
                try {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds);
                } catch (System.Data.SqlClient.SqlException ex) {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 获得IDataReader
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetReader(string strSQL) {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try {
                connection.Open();
                IDataReader myReader = cmd.ExecuteReader();
                return myReader;

            } catch (System.Data.SqlClient.SqlException e) {
                throw new Exception(e.Message);
            }
        }




        /// <summary>
        /// 获得单一字段
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>返回object</returns>
        public static object GetScalar(string SQLString) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection)) {
                    try {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))) {
                            return null;
                        } else {
                            return obj;
                        }
                    } catch (System.Data.SqlClient.SqlException e) {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        #endregion

    }


}
