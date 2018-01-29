using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="galleryid"></param>
        /// <returns></returns>
        public Entity.DetailGalleryInfo getDetailGalleryInfo(int galleryid)
        {
            return this.dbContext.DetailGallery.FirstOrDefault(g => g.GalleryId == galleryid);
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="detailid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Entity.DetailGalleryInfo getDetailGalleryInfo(int detailid, int index)
        {
            return this.getDetailGalleryInfo(detailid, null, index);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Entity.DetailGalleryInfo getDetailGalleryInfo(string tag, int index)
        {
            return this.getDetailGalleryInfo(0, tag, index);
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="detailid"></param>
        /// <param name="tag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Entity.DetailGalleryInfo getDetailGalleryInfo(int detailid, string tag, int index)
        {
            var lambda = PredicateExtensions.True<Entity.DetailGalleryInfo>();
            if (detailid > 0)
            {
                lambda = lambda.And(g => g.DetailId == detailid);
            }
            if (!string.IsNullOrEmpty(tag))
            {
                lambda = lambda.And(g => g.Tag.Equals(tag));
            }
           

            var list = this.dbContext.DetailGallery.Where(lambda).Skip(index).Take(1).ToList();
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public Entity.DetailGalleryInfo getDetailGalleryInfo(int detailid, Entity.GalleryType type)
        {
            return this.dbContext.DetailGallery.Where(g => g.DetailId == detailid && g.GalleryType == type).OrderBy(g => g.OrderNo).FirstOrDefault();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailGalleryInfo> getDetailGalleryList(int detailid, Entity.GalleryType gallerytype)
        {
            return this.getDetailGalleryList(0, detailid, gallerytype);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.DetailGalleryInfo> getDetailGalleryList(int top, int detailid, Entity.GalleryType gallerytype, bool iscover = false)
        {
            var lambda = PredicateExtensions.True<Entity.DetailGalleryInfo>();
            lambda = lambda.And(g => g.DetailId == detailid && g.GalleryType == gallerytype);
            if (iscover)
            {
                lambda = lambda.And(g => g.Cover == 1);
            }
            else
            {
                lambda = lambda.And(g => g.Cover == 0);
            }
            if (top <= 0)
                return this.dbContext.DetailGallery.Where(lambda).OrderBy(g => g.OrderNo).ToList();
            else
                return this.dbContext.DetailGallery.Where(lambda).OrderBy(g => g.OrderNo).Take(top).ToList();

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public List<Entity.DetailGalleryInfo> getDetailGalleryList(int detailid)
        {
            return this.dbContext.DetailGallery.Where(g => g.DetailId == detailid).OrderBy(g => g.Tag).OrderBy(g => g.OrderNo).ToList();
        }


        /// <summary>
        /// 更新图库
        /// </summary>
        /// <param name="info"></param>
        public void updateDetailGallery(Entity.DetailGalleryInfo info)
        {
            this.dbContext.Entry(info).State = System.Data.Entity.EntityState.Modified;
            this.dbContext.SaveChanges();
        }
        /// <summary>
        /// 更新图库
        /// </summary>
        /// <param name="list"></param>
        public void updateDetailGallery(List<Entity.DetailGalleryInfo> list, int detailid)
        {
            var oldlist = this.getDetailGalleryList(detailid);
            Dictionary<string, int> statistics = new Dictionary<string, int>();
            foreach (var info in list)
            {
                info.DetailId = detailid;
                if (info.GalleryId > 0)
                {
                    var currInfo = oldlist.Find(g => g.GalleryId == info.GalleryId);
                    if (currInfo != null)
                    {
                        info.AgreeNum = currInfo.AgreeNum;
                        info.FollowNum = currInfo.FollowNum;
                        info.InDate = currInfo.InDate;
                        info.HitsNum = currInfo.HitsNum;
                        oldlist.Remove(currInfo);
                        this.dbContext.Entry(currInfo).CurrentValues.SetValues(info);

                    }
                    else
                    {
                        info.InDate = Config.SiteConfig.getLocalTime();
                        this.dbContext.DetailGallery.Add(info);
                    }
                }
                else
                {
                    info.InDate = Config.SiteConfig.getLocalTime();
                    this.dbContext.DetailGallery.Add(info);
                }
                var key = "";
                if (info.Cover == 1)
                {
                    key = "cover";
                }
                else
                {
                    key = info.GalleryType.ToString();
                }
                if (statistics.ContainsKey(key))
                {
                    statistics[key]++;
                }
                else
                {
                    statistics.Add(key, 1);
                }
            }
            if (oldlist.Count > 0)
            {
                this.dbContext.DetailGallery.RemoveRange(oldlist);
            }

            this.dbContext.SaveChanges();

            if (statistics.Count > 0)
            {
                var detailInfo = this.getDetailInfo(detailid);
                var gallerid = 0;
                System.Text.StringBuilder galleryStatistics = new System.Text.StringBuilder();
                foreach (var galler in statistics)
                {
                    if (galler.Key == "cover" && gallerid == 0)
                    {
                        //封面
                        var coverInfo = this.dbContext.DetailGallery.Where(g => g.DetailId == detailid && g.Cover == 1).OrderBy(g => g.OrderNo).FirstOrDefault();
                        if (coverInfo != null)
                        {
                            detailInfo.GalleryId = coverInfo.GalleryId;
                        }
                    }
                    galleryStatistics.AppendFormat("{0}:{1};", galler.Key, galler.Value);
                }
                detailInfo.GalleryStatistics = galleryStatistics.ToString();

                this.dbContext.SaveChanges();
            }
            
        }
    }
}
