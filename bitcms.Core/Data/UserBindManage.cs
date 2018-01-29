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
        /// 获取列表
        /// </summary>
        public List<UserBindInfo> getUserBindList(PageInfo page, int userid)
        {
            var lambda = PredicateExtensions.True<Entity.UserBindInfo>();

            if (userid > 0)
                lambda = lambda.And(g => g.UserId == userid);

            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.UserCode.Contains(page.Key) || g.NickName.Contains(page.Key));
            }

            if (page.TotalCount == 0)
            {
                var list = this.dbContext.UserBind.Where(lambda).OrderByDescending(g => g.InDate);
                page.TotalCount = list.Count();
                return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return this.dbContext.UserBind.Where(lambda).OrderByDescending(g => g.InDate).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
        }

        /// <summary>
        /// 检查会员编码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int checkUserBindCode(string usercode)
        {
            return this.checkUserBindCode(null, usercode);
        }

        /// <summary>
        /// 检查会员账号
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int checkUserBindCode(string modulecode, string usercode)
        {
            var lambda = PredicateExtensions.True<Entity.UserBindInfo>();
            lambda = lambda.And(g => g.UserCode.Equals(usercode));

            if (!string.IsNullOrEmpty(modulecode))
            {
                lambda = lambda.And(g => g.ModuleCode.Equals(modulecode));
            }
           
            var info = this.dbContext.UserBind.FirstOrDefault(lambda);
            if (info != null)
            {
                return info.UserId;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取会员绑定账号
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Entity.UserBindInfo getUserBindInfo(string usercode)
        {
            return this.getUserBindInfo(null, usercode);
        }
        /// <summary>
        /// 获取会员绑定账号
        /// </summary>
        /// <param name="modulecode"></param>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Entity.UserBindInfo getUserBindInfo(string modulecode, string usercode)
        {
            var lambda = PredicateExtensions.True<Entity.UserBindInfo>();
            lambda = lambda.And(g => g.UserCode.Equals(usercode));
            if (!string.IsNullOrEmpty(modulecode))
            {
                lambda = lambda.And(g => g.ModuleCode.Equals(modulecode));
            }

                return this.dbContext.UserBind.FirstOrDefault(lambda);
        }
        /// <summary>
        /// 获取会员绑定账号
        /// </summary>
        /// <param name="modulecode"></param>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public Entity.UserBindInfo getUserBindInfo(string modulecode, int userid)
        {
            var lambda = PredicateExtensions.True<Entity.UserBindInfo>();
            lambda = lambda.And(g => g.UserId == userid&&g.ModuleCode.Equals(modulecode));

            return this.dbContext.UserBind.FirstOrDefault(lambda);
        }
        /// <summary>
        /// 获取会员绑定实体
        /// </summary>
        /// <param name="userbindid"></param>
        /// <returns></returns>
        public Entity.UserBindInfo getUserBindInfo(int userbindid)
        {
            return this.dbContext.UserBind.FirstOrDefault(g => g.UserBindId == userbindid);
        }
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <returns></returns>

        public Entity.UserBindInfo updateUserBind(string modulecode, int userid, string usercode, string nickname, int verify, string extend1 = null, string extend2 = null, string extend3 = null)
        {
            var info = this.getUserBindInfo(modulecode, usercode);
            if (info == null)
            {
                info = new Entity.UserBindInfo()
               {
                   ModuleCode = modulecode,
                   UserId = userid,
                   UserCode = usercode,
                   NickName = nickname,
                   Verify = verify,
                   Extend1 = extend1,
                   Extend2 = extend2,
                   Extend3 = extend3,
                   InDate = Config.SiteConfig.getLocalTime()
               };
                this.dbContext.UserBind.Add(info);
            }
            else
            {
                if (userid > 0 && info.UserId <= 0)
                {
                    info.UserId = userid;
                }
                info.NickName = nickname;
                info.Extend1 = extend1;
                info.Extend2 = extend2;
                info.Extend3 = extend3;
            }
            this.dbContext.SaveChanges();
            return info;
        }
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool updateUserBind(Entity.UserBindInfo info)
        {
            if (info.UserBindId <= 0)
            {
                this.updateUserBind(info.ModuleCode, info.UserId, info.UserCode, info.NickName, info.Verify, info.Extend1, info.Extend2, info.Extend3);
            }
            else
            {
                var _info = this.getUserBindInfo(info.UserBindId);
                if (_info != null)
                {
                    this.dbContext.Entry(_info).CurrentValues.SetValues(info);
                }
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 修改会员账号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void updateUserCode(int userid, string modulecode, string oldusercode, string usercode)
        {
            var userbindInfo = this.getUserBindInfo(modulecode, oldusercode);
            if (userbindInfo != null && userbindInfo.UserId == userid)
            {
                userbindInfo.UserCode = usercode;
                this.dbContext.SaveChanges();
            }
            else
            {
                Error = Entity.Error.错误;
                Message = "原账号不存在";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void deleteUserBind(string ids)
        {
            var _ids = ids.Split(',');
            var list = new List<Entity.UserBindInfo>();
            foreach (var id in _ids)
            {
                var _id = Utils.strToInt(id);
                var info = this.getUserBindInfo(_id);
                if (info != null)
                {
                    list.Add(info);
                }
            }
            this.dbContext.UserBind.RemoveRange(list);
            this.dbContext.SaveChanges();
        }
    }
}
