using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bitcms.DataProvider;
using bitcms.Entity;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 检查会员关注
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="followUserId"></param>
        /// <returns></returns>
        public bool checkFollow(int userid, int followUserId)
        {
            return this.dbContext.Follow.Count(g => g.UserId == userid && g.FollowUserId == followUserId) > 0;
        }
        /// <summary>
        /// 获取我的关注数 
        /// </summary>
        public int getFollowNum(int userid)
        {
            return this.dbContext.Follow.Count(g => g.UserId == userid);
        }
        /// <summary>
        /// 获取关注
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="followUserId"></param>
        /// <returns></returns>
        public Entity.FollowInfo getFollowInfo(int userid, int followUserId)
        {
            return this.dbContext.Follow.Find(userid, followUserId);
        }
        /// <summary>
        /// 插入会员关注
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followUserId"></param>
        /// <returns></returns>
        public bool insertFollowInfo(int userId, int followUserId)
        {
            var followinfo = this.getFollowInfo(userId, followUserId);
            if (followinfo != null)
            {
                return false;
            }
            followinfo = new Entity.FollowInfo()
            {
                UserId = userId,
                FollowUserId = followUserId,
                InDate = Config.SiteConfig.getLocalTime()
            };
            if (this.getFollowInfo(followUserId, userId) != null)
            {
                followinfo.Mutual = 1;
            }
            this.dbContext.Follow.Add(followinfo);
            var userinfo = this.getUserInfo(followUserId);
            if (userinfo != null)
            {
                userinfo.FollowNum++;
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除会员关注
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followUserId"></param>
        /// <returns></returns>
        public bool deleteFollowInfo(int userId, int followUserId)
        {
            var followinfo = this.getFollowInfo(userId, followUserId);
            if (followinfo != null)
            {
                this.dbContext.Follow.Remove(followinfo);
                var followuserinfo = this.getFollowInfo(followUserId, userId);
                if (followuserinfo != null)
                {
                    followuserinfo.Mutual = 0;
                }
                var userinfo = this.getUserInfo(followUserId);
                if (userinfo != null && userinfo.FollowNum > 0)
                {
                    userinfo.FollowNum--;
                }
                return this.dbContext.SaveChanges() > 0;
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "取消失败";
                return false;
            }
           
        }

        /// <summary>
        /// 获取我的关注列表
        /// </summary>
        public List<Entity.UserInfo> getFollowList(PageInfo page, int userid)
        {
            var linq = from u in this.dbContext.User
                       join f in this.dbContext.Follow on u.UserId equals f.FollowUserId
                       where f.UserId == userid
                       orderby f.InDate descending
                       select u;

            if (page.TotalCount == 0)
            {
                page.TotalCount = linq.Count();
                return linq.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return linq.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }

        }
        /// <summary>
        /// 获取我的粉丝列表
        /// </summary>
        public List<Entity.UserInfo> getFansList(PageInfo page, int userid)
        {
            var linq = from u in this.dbContext.User
                       join f in this.dbContext.Follow on u.UserId equals f.UserId
                       where f.FollowUserId == userid
                       orderby f.InDate descending
                       select u;

            if (page.TotalCount == 0)
            {
                page.TotalCount = linq.Count();
                return linq.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return linq.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }

        }
    }
}
