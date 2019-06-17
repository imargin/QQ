using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ
{
    class UserInfoDao
    {
        public static List<UserInfo> FindAll()
        {
            List<UserInfo> list = new List<UserInfo>();
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = DBUtil.GetConn();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select u.*, l.flag");
                sb.AppendLine("from userInfo u, level l");
                sb.AppendLine("where u.levelId = l.levelId");
                SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo u = new UserInfo();
                    u.Email = reader["email"].ToString();
                    u.Pwd = reader["userpwd"].ToString();
                    u.OnlineDay = Convert.ToDouble(reader["onlineDay"]);
                    u.Name = reader["username"].ToString();
                    u.LevelId = (int)reader["levelId"];
                    u.Id = (int)reader["userid"];
                    u.Flag = reader["flag"].ToString();
                    list.Add(u);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DBUtil.CloseAll(conn, reader);
            }

            return list;
        }

        internal void UpdateLevel()
        {
            DBUtil.ExecuteNonQuery("update userinfo set " +
                "levelId = (select max(levelId) from level " +
                "where onlineDays < onlineDay)", null);
        }

        internal UserInfo Get(int id)
        {
            UserInfo user = null;

            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = DBUtil.GetConn();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select *");
                sb.AppendLine("from userInfo");
                sb.AppendLine("where userid = {0}");
                string sql = string.Format(sb.ToString(), id);
                SqlCommand cmd = new SqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new UserInfo();
                    user.Email = reader["email"].ToString();
                    user.Pwd = reader["userpwd"].ToString();
                    user.OnlineDay = Convert.ToDouble(reader["onlineDay"]);
                    user.Name = reader["username"].ToString();
                    user.LevelId = (int)reader["levelId"];
                    user.Id = (int)reader["userid"];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                DBUtil.CloseAll(conn, reader);
            }

            return user;
        }

        internal int Delete(int id)
        {
            string sql = string.Format("delete from userInfo where userId={0}", 
                                                id);
            return DBUtil.ExecuteNonQuery(sql, new string[] { id + "" });
        }

        internal int Update(UserInfo u)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("update userInfo");
            sb.AppendLine("set userName='{0}', userPwd='{1}', levelId={2},");
            sb.AppendLine("     email='{3}', onlineDay={4}");
            sb.AppendLine("where userid={5}");
            return DBUtil.ExecuteNonQuery(sb.ToString(),
                    new string[] {u.Name, u.Pwd, u.LevelId + "",
                        u.Email, u.OnlineDay + "", u.Id + ""});
        }
    }
}
