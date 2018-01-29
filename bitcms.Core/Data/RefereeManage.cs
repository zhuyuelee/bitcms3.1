using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取我的推荐人
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="refereeid"></param>
        /// <returns></returns>
        public Entity.RefereeInfo getRefereeInfo(int userid)
        {
            return this.dbContext.Referee.FirstOrDefault(g => g.UserId == userid);
        }

        /// <summary>
        /// 获取我的推荐
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<Entity.RefereeInfo> getRefereelist(int userid)
        {
            return this.dbContext.Referee.Where(g => g.RefereeId == userid).ToList();
        }
        /// <summary>
        /// 插入我的推荐人
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="refereeid"></param>
        /// <param name="referee"></param>
        public bool insertReferee(int userid, int refereeid, string referee)
        {
            this.dbContext.Referee.Add(new Entity.RefereeInfo() { 
                UserId = userid,
                RefereeId = refereeid,
                Referee = referee,
                InDate = Config.SiteConfig.getLocalTime()
            });
            return this.dbContext.SaveChanges() > 0;
        }
    }
}
