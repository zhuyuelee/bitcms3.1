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
        /// 详情评论详情
        /// </summary>
        /// <param name="reviewid"></param>
        /// <returns></returns>
        public Entity.ReviewInfo getReviewInfo(int reviewid)
        {
            return this.dbContext.Review.FirstOrDefault(g => g.ReviewId == reviewid);
        }

        /// <summary>
        /// 更新评论点击次数
        /// </summary>
        public int updateReviewHitsNum(int reviewid)
        {
            var info = this.getReviewInfo(reviewid);
            if (info != null)
            {
                info.HitsNum++;
                this.dbContext.SaveChanges();
                return info.HitsNum;
            }
            else
            {
                Error = Entity.Error.错误;
                Message = "评论主体不存在";
                return 0;
            }
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="detailid"></param>
        /// <param name="content"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool addReview(Entity.ReviewInfo info)
        {
            if (info.DetailId > 0)
            {
                var detailinfo = this.getDetailInfo(info.DetailId);
                if(detailinfo!=null)
                {
                info.ChannelCode = detailinfo.ChannelCode;
                if (info.Verify == 1)
                {
                    detailinfo.ReviewNum++;
                }
                }
            }
            if (info.ReplyId > 0)
            {
                var reviewinfo = this.getReviewInfo(info.ReplyId);
                if (reviewinfo != null)
                {
                    reviewinfo.ReplyNum++;
                }
            }
            info.InDate = Config.SiteConfig.getLocalTime();
            this.dbContext.Review.Add(info);
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="reviewid"></param>
        /// <param name="content"></param>
        /// <param name="userid"></param>
        /// <param name="isadmin"></param>
        /// <returns></returns>
        public bool addReply(int reviewid, string content, int userid,int verify)
        {
            var info = this.getReviewInfo(reviewid);
            if (info != null)
            {
                var replyInfo = new Entity.ReviewInfo()
                {
                    AgainstNum = 0,
                    AgreeNum = 0,
                    ChannelCode = info.ChannelCode,
                    Content = content,
                    DetailId = info.DetailId,
                    InDate = Config.SiteConfig.getLocalTime(),
                    ReplyId = reviewid,
                    UserId = userid,
                    Verify = verify
                };

                //审核
                if (verify == 1)
                {
                    info.ReplyNum++;
                }
                this.dbContext.Review.Add(replyInfo);
                return this.dbContext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <returns></returns>
        public bool deleteReview(int reviewid)
        {
            var info = this.dbContext.Review.Find(reviewid);
            if (info != null)
            {
                if (info.ReplyId == 0)//删除评论
                {
                    if (info.Verify == 1 && info.DetailId > 0)
                    {
                        var detailInfo = getDetailInfo(info.DetailId);
                        if (detailInfo != null)
                        {
                            detailInfo.ReviewNum--;
                        }
                    }

                    var list = this.dbContext.Review.Where(g => g.ReplyId == reviewid);
                    if (list.Count() > 0)
                    {
                        this.dbContext.Review.RemoveRange(list);
                    }
                }
                else if (info.Verify == 1)
                {
                    //评论数减一
                    var reviewInfo = this.dbContext.Review.Find(info.ReplyId);
                    if (reviewInfo != null)
                    {
                        reviewInfo.ReplyNum--;
                    }
                }

                this.dbContext.SaveChanges();
                this.dbContext.Review.Remove(info);

                return this.dbContext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 审核评论
        /// </summary>
        /// <returns></returns>
        public bool verifyReview(int reviewid)
        {
            var info = this.dbContext.Review.Find(reviewid);
            if (info != null)
            {
                var detailInfo = getDetailInfo(info.DetailId);
                if (detailInfo != null)
                {
                    detailInfo.ReviewNum++;
                }
                if (info.ReplyId > 0)
                {
                    var reviewInfo = this.dbContext.Review.Find(info.ReplyId);
                    reviewInfo.ReplyNum++;
                }
                info.Verify = 1;
                return this.dbContext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="channelcode"></param>
        /// <param name="detailid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<UserReviewInfo> getReplyList(PageInfo page, string channelcode, int replyid, int userid)
        {
            return this.getReviewList(page, channelcode, 0, replyid, userid, 1);
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="channelcode"></param>
        /// <param name="detailid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<UserReviewInfo> getReviewList(PageInfo page, string channelcode, int detailid)
        {
            return this.getReviewList(page, channelcode, detailid, 0, 0, 1);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page">分页数据</param>
        /// <param name="channelcode">资讯类型</param>
        /// <param name="itemid">栏目id</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public List<UserReviewInfo> getReviewList(PageInfo page, string channelcode, int detailid,  int replyid, int userid, int verify)
        {
            var lambda = PredicateExtensions.True<UserReviewInfo>();

            lambda = lambda.And(g => g.ChannelCode == channelcode);

            //审核
            if (verify == 1)
            {
                lambda = lambda.And(g => g.Verify == 1);
            }

            if (detailid > 0)
                lambda = lambda.And(g => g.DetailId == detailid);

            if (userid > 0)
                lambda = lambda.And(g => g.UserId == userid);

            if (replyid > 0)
                lambda = lambda.And(g => g.ReplyId == replyid);
            else
            {
                lambda = lambda.And(g => g.ReplyId == 0);
            }
           
            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.Title.Contains(page.Key));
            }
            if (page.TotalCount == 0)
            {
                var list = this.dbContext.UserReview.Where(lambda).OrderByDescending(g => g.Show).ThenByDescending(g => g.ReviewId);
                page.TotalCount = list.Count();
                return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return this.dbContext.UserReview.Where(lambda).OrderByDescending(g => g.Show).ThenByDescending(g => g.ReviewId).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="top">几条数据</param>
        /// <param name="channelcode">类型</param>
        /// <param name="istop">置顶</param>
        /// <param name="isshow">推荐</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public List<UserReviewInfo> getReviewList(int top, string channelcode, bool isshow = false, string key = null)
        {
            var lambda = PredicateExtensions.True<UserReviewInfo>();

            lambda = lambda.And(g => g.ChannelCode == channelcode&&g.Verify == 1&&g.ReplyId == 0);


            

            if (isshow)
                lambda = lambda.And(g => g.Show == 1);

           
            if (!string.IsNullOrEmpty(key))
            {
                lambda = lambda.And(g => g.Title.Contains(key));
            }
            if (top > 0)
            {
                return this.dbContext.UserReview.Where(lambda).OrderByDescending(g => g.Show).ThenByDescending(g => g.ReviewId).Take(top).ToList();
            }
            else
            {
                return this.dbContext.UserReview.Where(lambda).OrderByDescending(g => g.Show).ThenByDescending(g => g.ReviewId).ToList();
            }
        }

        /// <summary>
        /// 获取我的评论列表
        /// </summary>
        /// <param name="page">分页数据</param>
        /// <param name="detailtype">资讯类型</param>
        /// <param name="itemid">栏目id</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public List<DetailReviewInfo> getDetailReviewList(PageInfo page, string channelcode, int detailid, int reviewid, int userid)
        {
            var lambda = PredicateExtensions.True<DetailReviewInfo>();

            if (!string.IsNullOrEmpty(channelcode))
            {
                lambda = lambda.And(g => g.ChannelCode == channelcode);
            }
            //会员
            if (userid > 0)
            {
                lambda = lambda.And(g => g.UserId == userid);
            }

            if (detailid > 0)
                lambda = lambda.And(g => g.DetailId == detailid);

            if (reviewid > 0)
                lambda = lambda.And(g => g.ReplyId == reviewid);
            else
            {
                lambda = lambda.And(g => g.ReplyId == 0);
            }
            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.Title.Contains(page.Key));
            }

            if (page.TotalCount == 0)
            {
                var list = this.dbContext.DetailReview.Where(lambda).OrderByDescending(g => g.Show).ThenByDescending(g => g.ReviewId);
                page.TotalCount = list.Count();
                return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return this.dbContext.DetailReview.Where(lambda).OrderByDescending(g => g.Show).ThenByDescending(g => g.ReviewId).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
        }


    }
}