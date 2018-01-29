using System;
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
        /// 检查会员阅读权限
        /// </summary>
        /// <returns></returns>
        public bool checkDetailPower(int detailid, int userid)
        {
            bool readpower = true;
            if (userid <= 0)//没有会员
            {
                readpower = false;
            }
            else
            {
                var detailReadPower = 0;
                var itemid = 0;
                var detailinfo = this.getDetailInfo(detailid);
                if (detailinfo != null)
                {
                    detailReadPower = detailinfo.ReadPower;
                    itemid = detailinfo.ItemId;
                }
                if (detailReadPower == 0 && itemid > 0)
                {
                    detailReadPower = this.getItemReadPower(itemid);
                }
                if (detailReadPower > 0)
                {

                    var userinfo = this.getUserInfo(userid);
                    var localtime = Config.SiteConfig.getLocalTime();
                    if (userinfo != null && userinfo.LockUser == 0 && userinfo.VerifyMember == 1 && (userinfo.Deadline == null || (userinfo.Deadline != null && userinfo.Deadline > localtime)))
                    {
                        var roleinfo = this.getRoleInfo(userinfo.RoleId);
                        if (roleinfo == null)//没有会员角色
                        {
                            readpower = false;
                        }
                        else if (detailReadPower > roleinfo.ReadPower)//阅读权限不够
                        {
                            readpower = false;
                        }
                    }
                    else
                    {
                        readpower = false;
                    }
                }
            }
            return readpower;
        }
        /// <summary>
        /// 检查编码
        /// </summary>
        /// <returns></returns>
        public int checkDetailCode(string detailcode, int detailid)
        {
            return this.dbContext.Detail.Count(g => g.DetailCode.Equals(detailcode) && g.DetailId != detailid);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(int itemid)
        {
            return getDetailList(0, null, itemid, 0, null);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(string channelcode, int show, string key)
        {
            return getDetailList(0, channelcode, 0, show, key);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(string channelcode, int itemid, int show, string key)
        {
            return getDetailList(0, channelcode, itemid, show, key);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(int top, int itemid, int show, string key)
        {
            return getDetailList(top, null, itemid, show, key);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(int itemid, int show, string key)
        {
            return getDetailList(0, null, itemid, show, key);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(int top, string channelcode, int itemid, int show, string key)
        {
            return getDetailList(top, channelcode, itemid, 0, show, key);

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(int top, string channelcode, int itemid, int userid, int show, string key)
        {
            var lambda = PredicateExtensions.True<Entity.DetailInfo>();

            var localtime = Config.SiteConfig.getLocalTime();
            lambda = lambda.And(g => g.Recycle != 1 && g.Display == 1 && g.DisplayTime < localtime);
            if (!string.IsNullOrEmpty(channelcode))
            {
                lambda = lambda.And(g => g.ChannelCode.Equals(channelcode));
            }
            if (itemid > 0)
            {
                var item = string.Format("|{0}|", itemid);
                lambda = lambda.And(g => g.Items.Contains(item));
            }
            if (userid > 0)
            {
                lambda = lambda.And(g => g.UserId == userid);
            }
            if (show > -1)
            {
                lambda = lambda.And(g => (g.Show & show) == show);
            }

            if (!string.IsNullOrEmpty(key))
            {
                lambda = lambda.And(g => g.SearchKey.Contains(key));
            }
            if (top > 0)
            {
                return this.dbContext.Detail.Where(lambda).OrderByDescending(g => g.Stars).ThenByDescending(g => g.DetailId).Take(top).ToList();
            }
            else
            {
                return this.dbContext.Detail.Where(lambda).OrderByDescending(g => g.Stars).ThenByDescending(g => g.DetailId).ToList();
            }
        }

        /// <summary>
        /// 获取点击排行列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getHotDetailList(int top, string channelcode, int days)
        {
            var key = string.Format("hotdetaillist_{0}_{1}_{2}", top, channelcode, days);
            var cacheData = Common.DataCache.get(key);
            if (cacheData != null)
            {
                return cacheData as List<Entity.DetailInfo>;
            }

            var startltime = Config.SiteConfig.getLocalTime();
            var enddays = startltime.AddDays(0 - days);
            var query = (from detail in this.dbContext.Detail
                         where detail.Recycle != 1 && detail.Display == 1 && detail.DisplayTime < startltime && detail.ChannelCode == channelcode
                         join hits in this.dbContext.DetailHits on detail.DetailId equals hits.DetailId
                         where hits.InDate >= enddays
                         group detail by detail.DetailId into result
                         orderby result.Count() descending
                         select new { detailid = result.Key, hitsnum = result.Count() }).Take(top);


            var resultlist = (from detail in this.dbContext.Detail
                              join q in query on detail.DetailId equals q.detailid
                              select new { detail = detail,hitsnum = q.hitsnum }).ToList();
            var list = resultlist.Select(x =>
            {
                x.detail.HitsNum = x.hitsnum;
                return new Entity.DetailInfo()
                        {
                            DetailId = x.detail.DetailId,
                            Title = x.detail.Title,
                            AgainstNum = x.detail.AgainstNum,
                            AgreeNum = x.detail.AgreeNum,
                            Author = x.detail.Author,
                            ChannelCode = x.detail.ChannelCode,
                            CloseReview = x.detail.CloseReview,
                            DetailCode = x.detail.DetailCode,
                            Display = x.detail.Display,
                            DisplayTime = x.detail.DisplayTime,
                            EnabledLink = x.detail.EnabledLink,
                            FollowNum = x.detail.FollowNum,
                            GalleryId = x.detail.GalleryId,
                            GalleryStatistics = x.detail.GalleryStatistics,
                            HitsNum = x.hitsnum,
                            InDate = x.detail.InDate,
                            ItemId = x.detail.ItemId,
                            Items = x.detail.Items,
                            Keyword = x.detail.Keyword,
                            Link = x.detail.Link,
                            ReadPower = x.detail.ReadPower,
                            Recycle = x.detail.Recycle,
                            RecycleDate = x.detail.RecycleDate,
                            Resume = x.detail.Resume,
                            ReviewNum = x.detail.ReviewNum,
                            SearchKey = x.detail.SearchKey,
                            Show = x.detail.Show,
                            Source = x.detail.Source,
                            Stars = x.detail.Stars,
                            SubTitle = x.detail.SubTitle,
                            UserId = x.detail.UserId
                        };
            }).ToList();


            //var startltime = Config.SiteConfig.getLocalTime();
            //var enddays = startltime.AddDays(0 - days);
            //var query = (from detail in this.dbContext.Detail
            //             where detail.Recycle != 1 && detail.Display == 1 && detail.DisplayTime < startltime && detail.ChannelCode == channelcode
            //             join hits in this.dbContext.DetailHits on detail.DetailId equals hits.DetailId
            //             where hits.InDate >= enddays
            //             group detail by detail.DetailId into result
            //             orderby result.Count() descending
            //             select new { detailid = result.Key, hitsnum = result.Count() }).Take(top).ToList();
            //var list = (from q in query
            //            join detail in this.dbContext.Detail on q.detailid equals detail.DetailId
            //            orderby q.hitsnum descending
            //            select new Entity.DetailInfo()
            //            {
            //                DetailId = detail.DetailId,
            //                Title = detail.Title,
            //                AgainstNum = detail.AgainstNum,
            //                AgreeNum = detail.AgreeNum,
            //                Author = detail.Author,
            //                ChannelCode = detail.ChannelCode,
            //                CloseReview = detail.CloseReview,
            //                DetailCode = detail.DetailCode,
            //                Display = detail.Display,
            //                DisplayTime = detail.DisplayTime,
            //                EnabledLink = detail.EnabledLink,
            //                FollowNum = detail.FollowNum,
            //                GalleryId = detail.GalleryId,
            //                GalleryStatistics = detail.GalleryStatistics,
            //                HitsNum = q.hitsnum,
            //                InDate = detail.InDate,
            //                ItemId = detail.ItemId,
            //                Items = detail.Items,
            //                Keyword = detail.Keyword,
            //                Link = detail.Link,
            //                ReadPower = detail.ReadPower,
            //                Recycle = detail.Recycle,
            //                RecycleDate = detail.RecycleDate,
            //                Resume = detail.Resume,
            //                ReviewNum = detail.ReviewNum,
            //                SearchKey = detail.SearchKey,
            //                Show = detail.Show,
            //                Source = detail.Source,
            //                Stars = detail.Stars,
            //                SubTitle = detail.SubTitle,
            //                UserId = detail.UserId
            //            }).ToList();
            Common.DataCache.set(key, list);
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(PageInfo page, string channelcode, int itemid)
        {
            return this.getDetailList(page, channelcode, itemid, 0);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(PageInfo page, string channelcode, int itemid, int userid)
        {
            return this.getDetailList(page, channelcode, itemid, userid, 1, 0, false);
        }

        public List<Entity.DetailInfo> getDetailList(PageInfo page, string channelcode, int itemid, int userid, int display, int show, bool recycle)
        {
            return this.getDetailList(page, channelcode, itemid, userid, display, show, -1, recycle);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(PageInfo page, string channelcode, int itemid, int userid, int display, int show, int unshow, bool recycle)
        {
            return this.getDetailList(page, channelcode, itemid, userid, display, show, unshow, -1, recycle);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public List<Entity.DetailInfo> getDetailList(PageInfo page, string channelcode, int itemid, int userid, int display, int show, int unshow, int gallerytype, bool recycle)
        {
            var lambda = PredicateExtensions.True<DetailInfo>();

            //回收站
            if (recycle)
            {
                lambda = lambda.And(g => g.Recycle == 1);
            }
            else
            {
                lambda = lambda.And(g => g.Recycle == 0);
            }
            if (!string.IsNullOrEmpty(channelcode))
            {
                lambda = lambda.And(g => g.ChannelCode == channelcode);
            }
            if (itemid > 0)
            {
                var item = string.Format("|{0}|", itemid);
                lambda = lambda.And(g => g.Items.Contains(item));
            }

            if (show > 0)
                lambda = lambda.And(g => (g.Show & show) == show);

            if (unshow > 0)
                lambda = lambda.And(g => (g.Show & show) != show);

            if (userid > 0)
            {
                lambda = lambda.And(g => g.UserId == userid);
            }
            if (display > -1)
            {
                lambda = lambda.And(g => g.Display == display);

                if (display == 1)
                {
                    var localtime = Config.SiteConfig.getLocalTime();
                    lambda = lambda.And(g => g.DisplayTime < localtime);
                }
            }
            if (gallerytype > -1)
            {
                var gtype = ((Entity.GalleryType)gallerytype).ToString() + ":";
                lambda = lambda.And(g => g.GalleryStatistics.Contains(gtype));

            }
            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.SearchKey.Contains(page.Key));
            }
            var list = this.dbContext.Detail.Where(lambda).OrderByDescending(g => g.Stars).ThenByDescending(g => g.DetailId);

            if (page.TotalCount == 0)
            {
                page.TotalCount = list.Count();
            }

            if (!string.IsNullOrEmpty(page.OrderField))
            {
                list = order(list, page.OrderField, page.SortOrder);
            }
            return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        private IOrderedQueryable<DetailInfo> order(IOrderedQueryable<DetailInfo> list, string orderfield, int sortorder)
        {
            switch (orderfield.ToLower())
            {
                case "hitsnum":
                    if (sortorder == 0)
                    {
                        return list.OrderBy(g => g.HitsNum);
                    }
                    else
                    {
                        return list.OrderByDescending(g => g.HitsNum);
                    }
                case "agreenum":
                    if (sortorder == 0)
                    {
                        return list.OrderBy(g => g.AgreeNum);
                    }
                    else
                    {
                        return list.OrderByDescending(g => g.AgreeNum);
                    }
                case "follownum":
                    if (sortorder == 0)
                    {
                        return list.OrderBy(g => g.FollowNum);
                    }
                    else
                    {
                        return list.OrderByDescending(g => g.FollowNum);
                    }

            }
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.UserDetailInfo> getUserDetailList(PageInfo page, string channelcode, int itemid)
        {
            return this.getUserDetailList(page, channelcode, itemid, 0, 0, 1);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.UserDetailInfo> getUserDetailList(PageInfo page, string channelcode, int itemid, int userid)
        {
            return this.getUserDetailList(page, channelcode, itemid, userid, 0, 1);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.UserDetailInfo> getUserDetailList(PageInfo page, string channelcode, int itemid, int userid, int show, int display)
        {
            return this.getUserDetailList(page, channelcode, itemid, userid, 0, 1, -1);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.UserDetailInfo> getUserDetailList(PageInfo page, string channelcode, int itemid, int userid, int show, int display, int gallerytype)
        {
            var lambda = PredicateExtensions.True<DetailInfo>();
            lambda = lambda.And(g => g.ChannelCode == channelcode && g.Recycle == 0);

            if (itemid > 0)
            {
                var item = string.Format("|{0}|", itemid);
                lambda = lambda.And(g => g.Items.Contains(item));
            }

            if (show > 0)
                lambda = lambda.And(g => (g.Show & show) == show);

            if (userid > 0)
            {
                lambda = lambda.And(g => g.UserId == userid);
            }
            if (display > -1)
            {
                lambda = lambda.And(g => g.Display == display);
            }
            if (gallerytype > -1)
            {
                var gtype = ((Entity.GalleryType)gallerytype).ToString() + ":";
                lambda = lambda.And(g => g.GalleryStatistics.Contains(gtype));

            }

            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.SearchKey.Contains(page.Key));
            }

            var list = this.dbContext.Detail.Where(lambda).OrderByDescending(g => g.Stars).ThenByDescending(g => g.DetailId);
            if (page.TotalCount == 0)
            {
                page.TotalCount = list.Count();
            }
            var orderlist = list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize);
            var trunlist = (from detail in orderlist
                            join user in dbContext.User on detail.UserId equals user.UserId
                            join item in dbContext.Item on detail.ItemId equals item.ItemId
                            select new
                            {
                                detail = detail,
                                user = user,
                                item = item
                            }).ToList();

            return trunlist.Select(x =>
                new Entity.UserDetailInfo()
                {
                    AgainstNum = x.detail.AgainstNum,
                    AgreeNum = x.detail.AgreeNum,
                    Author = x.detail.Author,
                    ChannelCode = x.detail.ChannelCode,
                    CloseReview = x.detail.CloseReview,
                    DetailCode = x.detail.DetailCode,
                    DetailId = x.detail.DetailId,
                    Display = x.detail.Display,
                    DisplayTime = x.detail.DisplayTime,
                    EnabledLink = x.detail.EnabledLink,
                    FollowNum = x.detail.FollowNum,
                    GalleryId = x.detail.GalleryId,
                    GalleryStatistics = x.detail.GalleryStatistics,
                    HitsNum = x.detail.HitsNum,
                    InDate = x.detail.InDate,
                    ItemId = x.detail.ItemId,
                    Items = x.detail.Items,
                    Keyword = x.detail.Keyword,
                    Link = x.detail.Link,
                    ReadPower = x.detail.ReadPower,
                    Recycle = x.detail.Recycle,
                    RecycleDate = x.detail.RecycleDate,
                    Resume = x.detail.Resume,
                    ReviewNum = x.detail.ReviewNum,
                    SearchKey = x.detail.SearchKey,
                    Show = x.detail.Show,
                    Source = x.detail.Source,
                    Stars = x.detail.Stars,
                    SubTitle = x.detail.SubTitle,
                    Title = x.detail.Title,
                    UserId = x.detail.UserId,

                    Avatar = x.user.Avatar,
                    UserName = x.user.UserName,
                    Icon = x.item.Icon,
                    ItemName = x.item.ItemName
                }
                ).ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.UserDetailInfo> getUserDetailList(int top, string channelcode, int itemid, int userid, int show, string key)
        {
            var lambda = PredicateExtensions.True<DetailInfo>();
            lambda = lambda.And(g => g.ChannelCode == channelcode && g.Recycle == 0);


            if (itemid > 0)
            {
                var item = string.Format("|{0}|", itemid);
                lambda = lambda.And(g => g.Items.Contains(item));
            }

            if (show > 0)
                lambda = lambda.And(g => (g.Show & show) == show);

            if (userid > 0)
            {
                lambda = lambda.And(g => g.UserId == userid);
            }

            if (!string.IsNullOrEmpty(key))
            {

                lambda = lambda.And(g => g.SearchKey.Contains(key));
            }

            var list = this.dbContext.Detail.Where(lambda).OrderByDescending(g => g.Stars).ThenByDescending(g => g.DetailId).Take(top);

            var trunlist = (from detail in list
                            join user in dbContext.User on detail.UserId equals user.UserId
                            join item in dbContext.Item on detail.ItemId equals item.ItemId
                            select new
                            {
                                detail = detail,
                                user = user,
                                item = item
                            }).ToList();

            return trunlist.Select(x =>
                new Entity.UserDetailInfo()
                {
                    AgainstNum = x.detail.AgainstNum,
                    AgreeNum = x.detail.AgreeNum,
                    Author = x.detail.Author,
                    ChannelCode = x.detail.ChannelCode,
                    CloseReview = x.detail.CloseReview,
                    DetailCode = x.detail.DetailCode,
                    DetailId = x.detail.DetailId,
                    Display = x.detail.Display,
                    DisplayTime = x.detail.DisplayTime,
                    EnabledLink = x.detail.EnabledLink,
                    FollowNum = x.detail.FollowNum,
                    GalleryId = x.detail.GalleryId,
                    GalleryStatistics = x.detail.GalleryStatistics,
                    HitsNum = x.detail.HitsNum,
                    InDate = x.detail.InDate,
                    ItemId = x.detail.ItemId,
                    Items = x.detail.Items,
                    Keyword = x.detail.Keyword,
                    Link = x.detail.Link,
                    ReadPower = x.detail.ReadPower,
                    Recycle = x.detail.Recycle,
                    RecycleDate = x.detail.RecycleDate,
                    Resume = x.detail.Resume,
                    ReviewNum = x.detail.ReviewNum,
                    SearchKey = x.detail.SearchKey,
                    Show = x.detail.Show,
                    Source = x.detail.Source,
                    Stars = x.detail.Stars,
                    SubTitle = x.detail.SubTitle,
                    Title = x.detail.Title,
                    UserId = x.detail.UserId,

                    Avatar = x.user.Avatar,
                    UserName = x.user.UserName,
                    Icon = x.item.Icon,
                    ItemName = x.item.ItemName
                }
                ).ToList();
        }

        /// <summary>
        /// 更新资讯
        /// </summary>
        /// <returns></returns>
        public bool updateDetail(Entity.DetailInfo info)
        {
            if (string.IsNullOrEmpty(info.DetailCode))
            {
                do
                {
                    info.DetailCode = Guid.NewGuid().ToString("N");
                } while (checkItemCode(info.DetailCode, info.ItemId) > 0);
            }

            var key = Utils.removeUnSafeString(info.Title);
            if (info.ItemId > 0)
            {
                var itemInfo = this.getItemInfo(info.ItemId);
                if (itemInfo != null)
                {
                    key += "," + Utils.removeUnSafeString(itemInfo.ItemName);
                }
                info.Items = getItems(info.ItemId);
            }
            if (!string.IsNullOrEmpty(info.Keyword))
            {
                info.Keyword = Utils.removeUnSafeString(info.Keyword);
            }
            if (string.IsNullOrEmpty(info.SearchKey))
            {
                info.SearchKey = string.Format("{0}|-_-|{1}", key, info.Keyword);
            }
            else
            {
                info.SearchKey = string.Format("{0},{2}|-_-|{1}", key, info.Keyword, info.SearchKey);
            }

            if (info.DetailId > 0)
            {
                var detailinfo = this.getDetailInfo(info.DetailId);
                if (info.InDate <= DateTime.MinValue)
                {
                    info.InDate = detailinfo.InDate;
                }
                if (info.DisplayTime <= DateTime.MinValue)
                {
                    info.DisplayTime = detailinfo.DisplayTime;
                }
                info.HitsNum = detailinfo.HitsNum;
                info.ReviewNum = detailinfo.ReviewNum;
                info.FollowNum = detailinfo.FollowNum;
                info.AgainstNum = detailinfo.AgainstNum;
                info.AgreeNum = detailinfo.AgreeNum;

                this.dbContext.Entry(detailinfo).CurrentValues.SetValues(info);
            }
            else
            {
                if (info.InDate <= DateTime.MinValue)
                {
                    info.InDate = Config.SiteConfig.getLocalTime();
                }
                if (info.DisplayTime <= DateTime.MinValue)
                {
                    info.DisplayTime = Config.SiteConfig.getLocalTime();
                }

                this.dbContext.Detail.Add(info);
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 获取栏目
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string getItems(int itemid)
        {
            System.Text.StringBuilder sbItems = new System.Text.StringBuilder();
            System.Text.StringBuilder sbItemNames = new System.Text.StringBuilder();
            while (itemid > 0)
            {
                var itemInfo = getItemInfo(itemid);
                if (itemInfo != null)
                {
                    itemid = itemInfo.FatherId;
                    sbItems.Insert(0, string.Format("|{0}|", itemInfo.ItemId));
                    sbItemNames.Insert(0, string.Format("{0}>", itemInfo.ItemName));
                }
                else
                {
                    itemid = 0;
                }
            };
            return string.Format("{0}|||{1}", sbItems.ToString(), sbItemNames.ToString());
        }
        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <returns></returns>
        public DetailInfo getDetailInfo(string code)
        {
            return this.dbContext.Detail.FirstOrDefault(g => g.DetailCode.Equals(code));
        }
        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="channelcode">频道编码</param>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public DetailInfo getDetailInfo(string channelcode, int detailid)
        {
            return this.dbContext.Detail.FirstOrDefault(g => g.ChannelCode.Equals(channelcode) && g.DetailId == detailid);
        }
        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public DetailInfo getDetailInfo(int detailid)
        {
            return this.dbContext.Detail.Find(detailid);
        }


        /// <summary>
        /// 获取下一篇内容
        /// </summary>
        /// <returns></returns>

        public DetailInfo getNextDetailInfo(int detailid, int itemid)
        {
            return this.dbContext.Detail.Where(g => g.DetailId > detailid && g.ItemId == itemid && g.Display == 1 && g.Recycle == 0).OrderBy(g => g.DetailId).FirstOrDefault();
        }
        /// <summary>
        /// 获取上一篇内容
        /// </summary>
        public DetailInfo getPrevDetailInfo(int detailid, int itemid)
        {
            return this.dbContext.Detail.Where(g => g.DetailId < detailid && g.ItemId == itemid && g.Display == 1 && g.Recycle == 0).OrderByDescending(g => g.DetailId).FirstOrDefault();
        }

        /// <summary>
        /// 更新点击次数
        /// </summary>
        public int updateDetailHitsNum(int detailid)
        {
            var info = this.getDetailInfo(detailid);
            if (info != null)
            {
                info.HitsNum++;
                this.dbContext.SaveChanges();

                return info.HitsNum;
            }
            else
            {
                Error = Entity.Error.错误;
                Message = "内容主体不存在";
                return 0;
            }
        }

        /// <summary>
        /// 更新收藏次数
        /// </summary>
        public int updateDetailFollowNum(int detailid)
        {
            var info = this.getDetailInfo(detailid);
            if (info != null)
            {
                info.FollowNum++;
                this.dbContext.SaveChanges();

                return info.FollowNum;
            }
            else
            {
                Error = Entity.Error.错误;
                Message = "内容主体不存在";
                return 0;
            }
        }

        /// <summary>
        /// 删除回收站
        /// </summary>
        public void deleteDetails(string ids)
        {
            var _ids = ids.Split(',');
            foreach (var id in _ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var info = this.getDetailInfo(Utils.strToInt(id));
                    if (info != null)
                    {
                        info.Recycle = 1;
                        info.RecycleDate = Config.SiteConfig.getLocalTime();
                    }
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void deleteDetail(int id)
        {
            if (id > 0)
            {
                var info = this.getDetailInfo(id);
                if (info != null)
                {
                    info.Recycle = 1;
                    info.RecycleDate = Config.SiteConfig.getLocalTime();
                }
                this.dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 还原回收站内容
        /// </summary>
        public void restoreDetails(string ids)
        {
            var _ids = ids.Split(',');
            foreach (var id in _ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var info = this.getDetailInfo(Utils.strToInt(id));
                    if (info != null)
                    {
                        info.Recycle = 0;
                        info.RecycleDate = null;
                    }
                }
                this.dbContext.SaveChanges();
            }
        }


        /// <summary>
        /// 删除回收站内容
        /// </summary>
        public void recycleDetails(string ids)
        {
            var _ids = ids.Split(',');
            foreach (var id in _ids)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var info = this.getDetailInfo(Utils.strToInt(id));
                    if (info != null)
                    {
                        this.dbContext.DetailGallery.RemoveRange(this.getDetailGalleryList(info.DetailId));
                        this.dbContext.DetailContent.RemoveRange(this.getDetailContentList(info.DetailId));
                        this.dbContext.Detail.Remove(info);
                    }
                }
                this.dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除回收站内容
        /// </summary>
        public void recycleDetail(int id)
        {
            if (id > 0)
            {
                var info = this.getDetailInfo(id);
                if (info != null)
                {
                    this.dbContext.DetailGallery.RemoveRange(this.getDetailGalleryList(info.DetailId));
                    this.dbContext.DetailContent.RemoveRange(this.getDetailContentList(info.DetailId));
                    this.dbContext.Detail.Remove(info);
                }
                this.dbContext.SaveChanges();
            }
        }

        #region 内容详情
        /// <summary>
        /// 获取内容详情列表
        /// </summary>
        public List<Entity.DetailContentInfo> getDetailContentList(int detailid)
        {
            return this.dbContext.DetailContent.Where(g => g.DetailId == detailid).OrderBy(g => g.OrderNo).ToList();
        }
        /// <summary>
        /// 获取内容详情列表
        /// </summary>
        /// <param name="code">频道编码</param>
        /// <param name="key">标题关键字</param>
        /// <returns></returns>
        public List<Entity.DetailContentInfo> getDetailContentList(string code, string key)
        {
            return this.dbContext.DetailContent.Where(g => g.ChannelCode.Equals(code) && g.Title.Contains(key)).OrderBy(g => g.OrderNo).ToList();
        }
        /// <summary>
        /// 获取内容详情
        /// </summary>
        public Entity.DetailContentInfo getDetailContentInfo(int detailid, int index)
        {
            var list = this.dbContext.DetailContent.Where(g => g.DetailId == detailid).OrderBy(g => g.OrderNo).Skip(index).Take(1).ToList();
            if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 更新内容详情
        /// </summary>
        /// <param name="list"></param>
        public void updateDetailContent(List<Entity.DetailContentInfo> list, int detailid, int itemid, string channelcode)
        {
            var oldlist = this.getDetailContentList(detailid);

            foreach (var info in list)
            {
                Entity.DetailContentInfo oldInfo = null;
                if (oldlist.Count > 0)
                {
                    oldInfo = oldlist[0];
                    oldlist.Remove(oldInfo);
                }
                else
                {
                    oldInfo = new Entity.DetailContentInfo();
                    oldInfo.InDate = Config.SiteConfig.getLocalTime();
                }
                oldInfo.DetailId = detailid;
                oldInfo.Content = info.Content;
                oldInfo.OrderNo = info.OrderNo;
                oldInfo.Title = info.Title;
                oldInfo.ItemId = itemid;
                oldInfo.ChannelCode = channelcode;

                if (oldInfo.ContentId <= 0)
                {
                    this.dbContext.DetailContent.Add(oldInfo);
                }

            }
            if (oldlist.Count > 0)
            {
                this.dbContext.DetailContent.RemoveRange(oldlist);
            }

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// 更新内容详情
        /// </summary>
        public void updateDetailContent(Entity.DetailContentInfo info)
        {
            if (info.ContentId <= 0)
            {
                info.InDate = Config.SiteConfig.getLocalTime();
                this.dbContext.DetailContent.Add(info);
            }
            else
            {
                this.dbContext.Entry<Entity.DetailContentInfo>(info).State = System.Data.Entity.EntityState.Modified;
            }
            this.dbContext.SaveChanges();
        }
        #endregion
    }
}
