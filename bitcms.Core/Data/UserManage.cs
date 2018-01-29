using System.Collections.Generic;
using System.Linq;
using bitcms.Common;
using bitcms.DataProvider;
using bitcms.Entity;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取会员实体
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public UserInfo getUserInfo(int userid)
        {
            return this.dbContext.User.Find(userid);
        }
        /// <summary>
        /// 获取会员积分
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int getUserScore(int userid)
        {
            if (userid > 0)
            {
                return this.dbContext.User.Where(g => g.UserId == userid).Sum(g => g.Score);
            }
            return 0;

        }

        /// <summary>
        /// 查询管理员登陆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public UserInfo checkLogin(string userName, string pwd, Entity.passwordType type)
        {
            var info = this.dbContext.User.FirstOrDefault(g => g.LockUser == 0 && g.UserName == userName);

            if (info != null)
            {
                //检查密码
                if (!this.checkUserPassword(info.UserId, pwd, type))
                {
                    info = null;
                }
                //最后登录时间
                updateLastLandDate(info);
            }

            return info;
        }

        /// <summary>
        /// 更新最后登录时间
        /// </summary>
        /// <param name="userinfo"></param>
        public void updateLastLandDate(Entity.UserInfo userinfo)
        {
            if (userinfo != null)
            {
                this.dbContext.Entry(userinfo).State = System.Data.Entity.EntityState.Modified;
                //最后登录时间
                userinfo.LastLandDate = Config.SiteConfig.getLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 检查会员
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int checkUserName(string username, int userid = -1)
        {
            var config = Config.SiteConfig.load();
            var lambda = PredicateExtensions.True<Entity.UserViewInfo>();
            lambda = lambda.And(g => g.UserName.Equals(username));
            if (userid > 0)
            {
                lambda = lambda.And(g => g.UserId != userid);
            }
            if (config.DuplicationUserName)
            {
                lambda = lambda.And(g => g.RoleType == "admin");
            }
            return this.dbContext.UserView.Where(lambda).Count();

        }

        /// <summary>
        /// 更新会员资料
        /// </summary>
        /// <param name="info"></param>
        public UserInfo updateUser(UserInfo info)
        {
            if (info.UserId <= 0)
            {
                this.dbContext.User.Add(info);
            }
            else
            {
                var userInfo = this.getUserInfo(info.UserId);

                info.InDate = userInfo.InDate;
                info.IP = userInfo.IP;
                info.Score = userInfo.Score;
                info.ComeFrom = userInfo.ComeFrom;
                info.FollowNum = userInfo.FollowNum;

                this.dbContext.Entry<Entity.UserInfo>(userInfo).CurrentValues.SetValues(info);
            }
            this.dbContext.SaveChanges();
            return info;

        }

        /// <summary>
        /// 更新会员资料
        /// </summary>
        /// <param name="info"></param>
        public UserInfo updateBasicUser(UserInfo info)
        {
            if (info.UserId > 0)
            {
                var userInfo = this.getUserInfo(info.UserId);
                info.InDate = userInfo.InDate;
                info.IP = userInfo.IP;
                info.Score = userInfo.Score;
                info.ComeFrom = userInfo.ComeFrom;

                info.Avatar = userInfo.Avatar;
                info.LockUser = userInfo.LockUser;
                info.RoleId = userInfo.RoleId;
                info.VerifyMember = userInfo.VerifyMember;

                info.Show = userInfo.Show;

                this.dbContext.Entry<Entity.UserInfo>(userInfo).CurrentValues.SetValues(info);
                this.dbContext.SaveChanges();
            }
            else
            {
                Error = Entity.Error.错误;
                Message = "会员不存在";
            }
            return info;
        }
        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="avatar"></param>
        public void updateUserAvatar(int userid, string avatar)
        {
            var userInfo = this.getUserInfo(userid);
            userInfo.Avatar = avatar;
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="roleType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<UserViewInfo> getUserList(int top, string roleType, string key, int roleid = 0, int show = 0, int locked = 0, int verify = 1)
        {
            var lambda = PredicateExtensions.True<Entity.UserViewInfo>();

            if (locked == 1)//锁定
            {
                lambda = lambda.And(g => g.LockUser == 1);
            }
            if (verify == 1)//审核
            {
                lambda = lambda.And(g => g.VerifyMember == 1);
            }
            if (roleid > 0)//角色
            {
                lambda = lambda.And(g => g.RoleId == roleid);
            }
            else if (!string.IsNullOrEmpty(roleType))
            {
                lambda = lambda.And(g => g.RoleType == roleType);
            }

            if (show > 0)
            {
                lambda = lambda.And(g => (g.Show & show) == show);
            }
            if (!string.IsNullOrEmpty(key))
            {
                lambda = lambda.And(g => g.UserName.Contains(key));
            }
            if (top > 0)
            {
                return this.dbContext.UserView.Where(lambda).OrderByDescending(g => g.UserId).Take(top).ToList();
            }
            else
            {
                return this.dbContext.UserView.Where(lambda).OrderByDescending(g => g.UserId).ToList();
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<UserViewInfo> getUserList(PageInfo page, string roleType,  int roleid = 0, int locked = 0, int verify = 1)
        {
            var lambda = PredicateExtensions.True<Entity.UserViewInfo>();
            if (locked == 1)//锁定
            {
                lambda = lambda.And(g => g.LockUser == 1);
            }
            if (verify == 1)//审核
            {
                lambda = lambda.And(g => g.VerifyMember == 1);
            }
            if (roleid > 0)
            {
                lambda = lambda.And(g => g.RoleId == roleid);
            }
            else if (!string.IsNullOrEmpty(roleType))
            {
                lambda = lambda.And(g => g.RoleType == roleType);
            }

            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.UserName.Contains(page.Key));

            }
            if (page.TotalCount == 0)
            {
                var list = this.dbContext.UserView.Where(lambda).OrderByDescending(g => g.UserId);
                page.TotalCount = list.Count();
                return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return this.dbContext.UserView.Where(lambda).OrderByDescending(g => g.UserId).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
        }


        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void deleteUsers(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string[] arrIds = ids.Split(',');
                var delList = new List<Entity.UserInfo>();
                foreach (string strId in arrIds)
                {
                    var info = this.getUserInfo(Utils.strToInt(strId));
                    if (info != null)
                    {
                        delList.Add(info);
                    }
                }
                this.dbContext.User.RemoveRange(delList);
                this.dbContext.SaveChanges();
            }

        }

        

    }
}
