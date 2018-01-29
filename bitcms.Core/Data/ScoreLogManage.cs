using System;
using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;
using bitcms.Entity;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 检查积分事件完成情况
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="eventcode"></param>
        /// <returns></returns>
        public bool checkScoreLog(int userid, string eventcode)
        { 
        if (userid <= 0)
            {
                return false;
            }
            var scoreEvent = this.dbContext.ScoreEvent.FirstOrDefault(g => g.EventCode == eventcode && g.Enabled == 1);

            if (scoreEvent != null)
            {
                //次数限制
                if (scoreEvent.Times > 0)
                {
                    var lambda = PredicateExtensions.True<Entity.ScoreLogInfo>();
                    lambda = lambda.And(g => g.EventId == scoreEvent.EventId);
                    if (scoreEvent.Period > 0)
                    {
                        DateTime perondDate = DateTime.Now.AddDays(0 - scoreEvent.Period);
                        lambda = lambda.And(g => g.InDate >= perondDate);
                    }
                    var count = this.dbContext.ScoreLog.Count(lambda);
                    if (count < scoreEvent.Times)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        

        /// <summary>
        /// 插入记录记录
        /// </summary>
        /// <param name="score"></param>
        /// <param name="eventcode"></param>
        /// <param name="reson"></param>
        /// <returns></returns>
        public bool insertScoreLog(int userid, int score, string eventcode, string reason)
        {
            if (userid <= 0)
            {
                return false;
            }
            var scoreEvent = this.dbContext.ScoreEvent.FirstOrDefault(g => g.EventCode == eventcode && g.Enabled == 1);

            if (scoreEvent != null)
            {
                //次数限制
                if (scoreEvent.Times > 0)
                {
                    var lambda = PredicateExtensions.True<Entity.ScoreLogInfo>();
                    lambda = lambda.And(g => g.EventId == scoreEvent.EventId && g.UserId == userid);
                    if (scoreEvent.Period > 0)
                    {
                        DateTime perondDate = DateTime.Now.AddDays(0 - scoreEvent.Period);
                        lambda = lambda.And(g => g.InDate >= perondDate);
                    }

                    var count = this.dbContext.ScoreLog.Count(lambda);
                    if (count >= scoreEvent.Times)
                    {
                        return false;
                    }
                }

                if (score <= 0)
                {
                    score = scoreEvent.Score;
                }
                if (string.IsNullOrEmpty(reason))
                {
                    reason = scoreEvent.EventName;
                }
                var logInfo = new Entity.ScoreLogInfo()
                {
                    IncomeOrExpenses = scoreEvent.Direction,
                    EventId = scoreEvent.EventId,
                    InDate = DateTime.Now,
                    Reason = reason,
                    UserId = userid,
                    Score = score,
                };
                this.dbContext.ScoreLog.Add(logInfo);

                var userinfo = this.getUserInfo(logInfo.UserId);
                if (userinfo != null)
                {
                    if (scoreEvent.Direction == 1)
                    {
                        userinfo.Score -= logInfo.Score;
                    }
                    else
                    {
                        userinfo.Score += logInfo.Score;
                    }
                    //角色 
                    var userroleinfo = this.getRoleInfo(userinfo.RoleId);
                    if (userroleinfo.RoleType == "score")
                    {
                        var roleInfo = this.getRoleByScore("score", userinfo.Score);
                        if (roleInfo != null && userinfo.RoleId != roleInfo.RoleId)
                        {
                            userinfo.RoleId = roleInfo.RoleId;
                        }
                    }
                }


                return this.dbContext.SaveChanges() > 0;
            }
            else
            {
                Common.Logs.error("scorelog", eventcode + "积分事件没启用或不存在");
                return false;
            }
        }

        /// <summary>
        /// 插入记录记录
        /// </summary>
        /// <param name="score"></param>
        /// <param name="eventcode"></param>
        /// <returns></returns>
        public bool insertScoreLog(int userid, string eventcode)
        {
            return insertScoreLog(userid, 0, eventcode, "");
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<Entity.ScoreLogsInfo> getScoreLogList(PageInfo page, int userid)
        {
            var lambda = PredicateExtensions.True<Entity.ScoreLogInfo>();

            if (userid > 0)
                lambda = lambda.And(g => g.UserId == userid);

            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.Reason.Contains(page.Key));
            }


            IQueryable<ScoreLogInfo> pagelist = null;

            if (page.TotalCount == 0)
            {
                var list = this.dbContext.ScoreLog.Where(lambda).OrderByDescending(g => g.LogId);
                page.TotalCount = list.Count();
                pagelist = list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize);
            }
            else
            {
                pagelist = this.dbContext.ScoreLog.Where(lambda).OrderByDescending(g => g.LogId).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize);
            }

            var turnlist = (from l in pagelist
                            join u in this.dbContext.User on l.UserId equals u.UserId
                            orderby l.LogId descending
                            select new
                            {
                                l.UserId,
                                l.Score,
                                l.Reason,
                                l.LogId,
                                l.InDate,
                                l.IncomeOrExpenses,
                                l.EventId,
                                u.UserName
                            }).ToList();

            var loglist = new List<Entity.ScoreLogsInfo>();
            turnlist.ForEach(g =>
            {
                loglist.Add(new Entity.ScoreLogsInfo()
                {
                    UserId = g.UserId,
                    Score = g.Score,
                    Reason = g.Reason,
                    LogId = g.LogId,
                    IncomeOrExpenses = g.IncomeOrExpenses,
                    EventId = g.EventId,
                    InDate = g.InDate,
                    UserName = g.UserName
                });
            });

            return loglist;
        }
    }
}
