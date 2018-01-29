using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;
using bitcms.Entity;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取积分事件实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScoreEventInfo getScoreEventInfo(int eventid)
        {
            return this.dbContext.ScoreEvent.Find(eventid);
        }

        /// <summary>
        /// 检查编码
        /// </summary>
        /// <returns></returns>
        public bool checkScoreEventCode(int eventid, string eventcode)
        {
            return this.dbContext.ScoreEvent.Count(g => g.EventCode == eventcode && g.EventId != eventid) > 0;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ScoreEventInfo> getScoreEventList(PageInfo page, int eventType)
        {
            var lambda = PredicateExtensions.True<Entity.ScoreEventInfo>();

            if (eventType > -1)
                lambda = lambda.And(g => g.EventType == eventType);

            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.EventName.Contains(page.Key));
            }

            if (page.TotalCount == 0)
            {
                var list = this.dbContext.ScoreEvent.Where(lambda).OrderByDescending(g => g.EventId);
                page.TotalCount = list.Count();
                return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return this.dbContext.ScoreEvent.Where(lambda).OrderByDescending(g => g.EventId).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        public List<ScoreEventInfo> getScoreEventList( int eventType)
        {
            var lambda = PredicateExtensions.True<Entity.ScoreEventInfo>();

            lambda = lambda.And(g => g.Enabled == 1);
            if (eventType > -1)
                lambda = lambda.And(g => g.EventType == eventType);

            return this.dbContext.ScoreEvent.Where(lambda).OrderByDescending(g => g.EventId).ToList();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool updateScoreEvent(Entity.ScoreEventInfo info)
        {
            if (info.EventId <= 0)
            {
                this.dbContext.ScoreEvent.Add(info);
            }
            else
            {
                this.dbContext.Entry<Entity.ScoreEventInfo>(info).State = System.Data.Entity.EntityState.Modified;
                this.dbContext.Entry<Entity.ScoreEventInfo>(info).Property(g => g.EventCode).IsModified = false;
                this.dbContext.Entry<Entity.ScoreEventInfo>(info).Property(g => g.EventType).IsModified = false;
            }

            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除积分事件
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public bool deleteScoreEvent(int eventid)
        {
            var eventInfo = this.getScoreEventInfo(eventid);
            if (eventInfo != null && eventInfo.EventType != 0)
            {
                var count = this.dbContext.ScoreLog.Count(g => g.EventId == eventInfo.EventId);
                if (count == 0)
                {
                    this.dbContext.ScoreEvent.Remove(eventInfo);
                    this.dbContext.SaveChanges();
                }
                else
                {
                    this.Error = Entity.Error.错误;
                    this.Message = "该积分事件已经在使用中，删除失败";
                }
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "删除错误，该积分事件不存在或是系统集成事件！";
            }
            return false;
        }
    }
}