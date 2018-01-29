using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bitcms.Common;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 字符转列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<int> stringToList(string ids)
        {
            List<int> list = new List<int>();
            var _ids = ids.Split(',');
            foreach (var id in _ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    list.Add(Utils.strToInt(id));
                }
            }

            return list;
        }

        /// <summary>
        /// 获取点赞列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="detailids"></param>
        /// <param name="reviewids"></param>
        /// <returns></returns>
        public List<Entity.ViewPointInfo> getViewPointList(int userid, string detailids, string reviewids)
        {
            var lambda = PredicateExtensions.True<Entity.ViewPointInfo>();
            lambda = lambda.And(g => g.UserId == userid);
            if (!string.IsNullOrEmpty(detailids))
            {
                var ids = stringToList(detailids);
                lambda = lambda.And(g => ids.Contains(g.DetailId));
            }
            else if (!string.IsNullOrEmpty(reviewids))
            {
                var ids = stringToList(reviewids);
                lambda = lambda.And(g => ids.Contains(g.ReviewId));
            }
            return this.dbContext.ViewPoint.Where(lambda).ToList();
        }

        /// <summary>
        /// 文章点赞
        /// </summary>
        /// <param name="detailid"></param>
        /// <param name="agree"></param>
        /// <param name="against"></param>
        /// <returns></returns>
        private int setDetailViewPoint(int detailid, bool addPoint, int agree = -1, int against = -1)
        {
            var num = 0;
            var info = this.getDetailInfo(detailid);
            if (info != null)
            {
                if (agree == 1)
                {
                    if (addPoint)
                        num = info.AgreeNum += 1;
                    else
                        num = info.AgreeNum -= 1;
                    if (info.AgreeNum < 0)
                    {
                        num = info.AgreeNum = 0;
                    }
                }
                if (against == 1)
                {
                    if (addPoint)
                        num = info.AgainstNum += 1;
                    else
                        num = info.AgainstNum -= 1;
                    if (info.AgainstNum < 0)
                    {
                        num = info.AgreeNum = 0;
                    }
                }
                this.dbContext.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 评论点赞
        /// </summary>
        /// <param name="detailid"></param>
        /// <param name="agree"></param>
        /// <param name="against"></param>
        /// <returns></returns>
        private int setReviewViewPoint(int reviewid, bool addPoint, int agree = -1, int against = -1)
        {
            var num = 0;
            var info = this.getReviewInfo(reviewid);
            if (info != null)
            {
                if (agree == 1)
                {
                    if (addPoint)
                        num = info.AgreeNum += 1;
                    else
                        num = info.AgreeNum -= 1;
                    if (info.AgreeNum < 0)
                    {
                        num = info.AgreeNum = 0;
                    }
                }
                if (against > -1)
                {
                    if (addPoint)
                        num = info.AgainstNum += 1;
                    else
                        num = info.AgainstNum -= 1;
                    if (info.AgainstNum < 0)
                    {
                        num = info.AgreeNum = 0;
                    }
                }
                this.dbContext.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 表达观点
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="detailid"></param>
        /// <param name="reviewid"></param>
        public int setViewPoint(int userid, int detailid, int reviewid, int agree = -1, int against = -1)
        {
            var lambda = PredicateExtensions.True<Entity.ViewPointInfo>();
            lambda = lambda.And(g => g.UserId == userid);
            if (detailid > 0)
            {
                lambda = lambda.And(g => g.DetailId == detailid);
            }
            if (reviewid > 0)
            {
                lambda = lambda.And(g => g.ReviewId == reviewid);
            }
            var addPoint = true;
            var viewPointInfo = this.dbContext.ViewPoint.FirstOrDefault(lambda);
            if (viewPointInfo == null)
            {
                viewPointInfo = new Entity.ViewPointInfo()
                {
                    UserId = userid,
                    ReviewId = reviewid,
                    DetailId = detailid,
                    InDate = Config.SiteConfig.getLocalTime(),
                };
                this.dbContext.ViewPoint.Add(viewPointInfo);
            }
            else
            {
                addPoint = false;
                this.dbContext.ViewPoint.Remove(viewPointInfo);
            }
            int num = 0;
            if (agree == 1)
            {
                viewPointInfo.Agree = agree;

                if (reviewid > 0)
                {
                    num = this.setReviewViewPoint(reviewid, addPoint, agree: agree);
                }
                else if (detailid > 0)
                {
                    num = this.setDetailViewPoint(detailid, addPoint, agree: agree);
                }
            }
            if (against == 1)
            {
                viewPointInfo.Against = against;
                if (reviewid > 0)
                {
                    num = this.setReviewViewPoint(reviewid, addPoint, against: against);
                }
                else if (detailid > 0)
                {
                    num = this.setDetailViewPoint(reviewid, addPoint, against: against);
                }
            }
            this.dbContext.SaveChanges();

            return num;
        }

    }
}
