using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ
{
    class AdminDao
    {
        public bool Login(string loginId, string loginPwd)
        {
            int r = 0;
            SqlConnection conn = DBUtil.GetConn();
            try
            {
                string sql = string.Format("select count(0) from admin " +
                                "where loginId='{0}' and loginPwd = '{1}'",
                                loginId, loginPwd);
                SqlCommand cmd = new SqlCommand(sql, conn);
                r = (int)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DBUtil.CloseAll(conn, null);
            }
            return r > 0;
        }
    }
}
