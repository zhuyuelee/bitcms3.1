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
        /// 插入点击
        /// </summary>
        /// <param name="useronlineid"></param>
        /// <param name="userid"></param>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public int insertDetailHits(string useronlineid, int userid, int detailid)
        {
            return this.insertDetailHits(new Entity.DetailHitsInfo()
            {
                UserOnlineId = useronlineid,
                DetailId = detailid,
                UserId = userid
            });
        }
        /// <summary>
        /// 插入点击
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int insertDetailHits(Entity.DetailHitsInfo info)
        {
            if (info.DetailId > 0 && !string.IsNullOrEmpty(info.UserOnlineId))
            {
                info.DetailHitsCode = Guid.NewGuid().ToString("N");
                info.InDate = bitcms.Config.SiteConfig.getLocalTime();
                this.dbContext.DetailHits.Add(info);
                if (this.dbContext.SaveChanges() > 0)
                {
                   return this.updateDetailHitsNum(info.DetailId);
                }
            }
            return 0;
        }
    }
}
