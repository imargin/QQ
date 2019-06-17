using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ
{
    class Menu
    {
        private AdminDao adminDao = new AdminDao();
        private UserInfoDao userInfoDao = new UserInfoDao();

        public  void ShowMenu()
        {
            if (!Login())
            {
                return;
            }
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("============欢迎登录 QQ 用户信息管理系统============");
                Console.WriteLine("---------------------请选择操作键-------------------");
                Console.WriteLine("1. 显示用户信息");
                Console.WriteLine("2. 更新在线天数");
                Console.WriteLine("3. 添加用户");
                Console.WriteLine("4. 更新用户等级");
                Console.WriteLine("5. 删除用户");
                Console.WriteLine("6. 退出");
                Console.WriteLine("====================================================");
                Console.Write("请选择：");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowUserInfos();
                        break;
                    case "2":
                        UpdateOnlineDay();
                        break;
                    case "3":
                        Console.WriteLine("3. 添加用户");
                        break;
                    case "4":
                        userInfoDao.UpdateLevel();
                        break;
                    case "5":
                        Delete();
                        break;
                    case "6":
                        flag = false;
                        Console.WriteLine("谢谢使用");
                        break;
                    default:
                        Console.WriteLine("请输入正确的选项(1~6)。");
                        break;
                }
                Console.WriteLine();
            }
        }

        private void Delete()
        {
            Console.Write("请输入用户编号：");
            int id = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            int r = userInfoDao.Delete(id);
            if (r > 0)
            {
                Console.WriteLine("删除成功");
            }
            else
            {
                Console.WriteLine("删除失败");
            }
        }

        private void UpdateOnlineDay()
        {
            Console.Write("请输入用户编号：");
            int id = int.Parse(Console.ReadLine());
            Console.Write("请输入新的在线天数：");
            double onlineDays = double.Parse(Console.ReadLine());

            // 根据 id 查询用户
            UserInfo u = userInfoDao.Get(id);
            if(u == null)
            {
                Console.WriteLine("该用户不存在");
                return;
            }

            // 更新在线天数
            u.OnlineDay = onlineDays;
            int r = userInfoDao.Update(u);

            if(r > 0)
            {
                Console.WriteLine("更新成功");
            }
            else
            {
                Console.WriteLine("更新失败");
            }
        }

        private void ShowUserInfos()
        {
            List<UserInfo> list = UserInfoDao.FindAll();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("编号\t昵称\t等级\t邮箱\t在线天数");
            foreach(UserInfo u in list)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", 
                    u.Id, u.Name, u.Flag, u.Email, u.OnlineDay);
            }
            Console.WriteLine("---------------------------------------------------------");
        }

        public bool Login()
        {
            bool r = false;
            for (int i = 2; i >= 0; i--)
            {
                Console.Write("请输入用户名：");
                string loginId = Console.ReadLine();
                Console.Write("请输入密码：");
                string loginPwd = Console.ReadLine();
                r = adminDao.Login(loginId, loginPwd);
                if (r)
                {
                    Console.WriteLine("登录成功");
                    break;
                }
                else if(i > 0) { 
                    Console.WriteLine("用户名或密码错误，你还有 {0} 次机会", i);
                }
                else
                {
                    Console.WriteLine("3 次输入均不正确，登录失败");
                }    
            }
            return r;
        }
    }
}
