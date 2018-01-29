using System.Linq;
using bitcms.Common;
using bitcms.DataProvider;
using bitcms.Entity;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        #region 密码操作
        /// <summary>
        /// 检查会员密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkUserPassword(int userid, string password, Entity.passwordType type)
        {
            var checkPwd = Utils.bitcmsMD5(password);
            return (from u in this.dbContext.User
                    join p in this.dbContext.UserPassword on u.UserId equals p.UserId
                    where u.UserId == userid && u.LockUser == 0 && p.PasswordType == type && p.Password.Equals(checkPwd)
                    select u).Count() > 0;
        }

        /// <summary>
        /// 获取密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="passwordtype"></param>
        /// <returns></returns>
        private UserPasswordInfo getUserPasswordInfo(int userid, Entity.passwordType passwordtype)
        {
            return this.dbContext.UserPassword.FirstOrDefault(g => g.UserId == userid && g.PasswordType == passwordtype);
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="passwordtype"></param>
        /// <param name="password"></param>
        private void changePassword(int userid, Entity.passwordType passwordtype, string password, bool encrypt = true)
        {
            if (encrypt)
            {
                password = Utils.bitcmsMD5(password);
            }
            var passwordInfo = this.getUserPasswordInfo(userid, passwordtype);
            if (passwordInfo == null)
            {
                passwordInfo = new UserPasswordInfo()
                {
                    UserId = userid,
                    Password = password,
                    PasswordType = passwordtype,
                    LastDate = Config.SiteConfig.getLocalTime()
                };
                this.dbContext.UserPassword.Add(passwordInfo);
            }
            else
            {
                passwordInfo.Password = password;
                passwordInfo.LastDate = Config.SiteConfig.getLocalTime();
            }

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新管理员密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        public void updateAdminPassword(int userid)
        {
            var adminPasswordInfo = this.getUserPasswordInfo(userid, Entity.passwordType.manage);
            if (adminPasswordInfo == null)
            {
                var userPasswordInfo = this.getUserPasswordInfo(userid, Entity.passwordType.user);
                if (userPasswordInfo != null)
                {
                    this.changePassword(userid, Entity.passwordType.manage, userPasswordInfo.Password, false);
                }

            }
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        public void updatePassword(int userid, Entity.passwordType passwordtype, string password)
        {
            this.updatePassword(userid, passwordtype, null, password);
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="newpassword"></param>
        public void updatePassword(int userid, Entity.passwordType passwordtype, string password, string newpassword)
        {
            var passwordInfo = this.getUserPasswordInfo(userid, passwordtype);
            if (passwordInfo != null)
            {
                this.dbContext.Entry<Entity.UserPasswordInfo>(passwordInfo).State = System.Data.Entity.EntityState.Modified;
                if (!string.IsNullOrEmpty(password))
                {
                    if (passwordInfo.Password.Equals(Utils.bitcmsMD5(password)))
                    {
                        this.changePassword(userid, passwordtype, newpassword);
                    }
                    else
                    {
                        Error = Entity.Error.错误;
                        Message = "原密码不正确";
                    }
                }
                else
                {
                    this.changePassword(userid, passwordtype, newpassword);
                }
            }
            else
            {
                this.changePassword(userid, passwordtype, newpassword);
            }
        }

        #endregion
    }
}
