using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public class MapRouteManage : DataBase
    {
        public MapRouteManage() : base(false) { }
        /// <summary>
        /// 获取字节点
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public List<Entity.MapRouteInfo> getList(int fatherid)
        {
            return dbContext.MapRoute.Where(g => g.FatherId == fatherid).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.MapRouteInfo getInfo(int id)
        {
            return dbContext.MapRoute.Find(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void delete(int id)
        {
            var list = this.getList(id);
            if (list.Count == 0)
            {
                var info = this.dbContext.MapRoute.Find(id);
                this.dbContext.MapRoute.Remove(info);
                if (this.dbContext.SaveChanges() > 0)
                {
                    updateBasicMapRouteConfig();
                }
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "该记录包含子重写记录";
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        public bool update(Entity.MapRouteInfo info)
        {
            if (info.MapRouteId <= 0)
            {
                this.dbContext.MapRoute.Add(info);
            }
            else
            {
                this.dbContext.Entry(info).State = System.Data.Entity.EntityState.Modified;
            }
            if (this.dbContext.SaveChanges() > 0)
            {
                updateBasicMapRouteConfig();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存到xml文件
        /// </summary>
        /// <param name="list"></param>
        public void updateBasicMapRouteConfig()
        {
            var list = this.dbContext.MapRoute.Where(g => g.Enabled == 1).ToList();
            var childList = new List<Entity.BasicMapRouteInfo>();

            list.ForEach(g =>
            {
                var lookfor = g.LookFor;
                if (!string.IsNullOrEmpty(lookfor) && lookfor.IndexOf('\r') > -1)
                {
                    lookfor = lookfor.Replace("\r\n", "\n");
                    lookfor = lookfor.Replace('\r', '\n');
                }
                var sendto = g.SendTo;
                if (!string.IsNullOrEmpty(sendto) && sendto.IndexOf('\r') > -1)
                {
                    sendto = sendto.Replace("\r\n", "\n");
                    sendto = sendto.Replace('\r', '\n');
                }
                childList.Add(new Entity.BasicMapRouteInfo()
                {
                    LookFor = lookfor,
                    SendTo = sendto,
                    Template = g.Template,
                    CacheTime = g.CacheTime
                });
            });

            Config.MapRouteConfig.save(childList);

        }
    }
}
