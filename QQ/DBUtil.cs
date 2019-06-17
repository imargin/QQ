using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ
{
    internal static class DBUtil
    {
        private static string connStr = "Data Source=HASEE-K650;Initial Catalog=QQ;Integrated Security=True;";

        public static SqlConnection GetConn()
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return conn;
        }

        public static void CloseAll(SqlConnection conn, SqlDataReader reader)
        {
            try
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static int ExecuteNonQuery(string sql, string[] ps)
        {
            int r = 0;
            SqlConnection conn = GetConn();

            if (ps != null && ps.Length > 0)
            {
                sql = string.Format(sql, ps);
            }
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                r = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                CloseAll(conn, null);
            }

            return r;
        }
    }
}
