using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : bitcms.DataProvider.DataBase
    {
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.AdsInfo> getAdsList()
        {
            return this.dbContext.Ads.ToList();
        }

        /// <summary>
        /// 获取广告详情列表 
        /// </summary>
        /// <param name="code">小写</param>
        /// <returns></returns>
        public List<Entity.AdsDetailInfo> getAdsDetailList(int top, string code, string tag, bool islimitDate = true)
        {
            var lambda = PredicateExtensions.True<Entity.AdsDetailInfo>();
            lambda = lambda.And(g => g.AdsCode.Equals(code));
            if (!string.IsNullOrEmpty(tag))
            {
                lambda = lambda.And(g => g.Tag.Equals(tag));
            }
            if (islimitDate)
            {
                var date = Config.SiteConfig.getLocalTime();
                lambda = lambda.And(g => g.StartDate < date && g.EndDate > date);
            }
            var list = this.dbContext.AdsDetail.Where(lambda);
            if (top > 0)
            {
                list = list.Take(top);
            }
            if (islimitDate)
            {
                foreach (var info in list)
                {
                    info.ShowNum += 1;//显示次数
                }
                this.dbContext.SaveChanges();
            }
            return list.OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 获取广告详情列表 
        /// </summary>
        /// <param name="code">小写</param>
        /// <returns></returns>
        public List<Entity.AdsDetailInfo> getAdsDetailList(int top, string code)
        {
            return getAdsDetailList(top, code, null);
        }

        /// <summary>
        /// 获取广告详情列表 
        /// </summary>
        /// <param name="code">小写</param>
        /// <returns></returns>
        public List<Entity.AdsDetailInfo> getAdsDetailList(string code)
        {
            return getAdsDetailList(0, code, null);
        }
        /// <summary>
        /// 获取广告详情实体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Entity.AdsDetailInfo getAdsDetailInfo(string code)
        {
            return this.getAdsDetailList(1, code).FirstOrDefault();
        }
        /// <summary>
        /// 获取广告实体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Entity.AdsInfo getAdsInfo(string code)
        {
            return this.dbContext.Ads.FirstOrDefault(g => g.AdsCode.Equals(code));
        }
        /// <summary>
        /// 获取广告实体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Entity.AdsInfo getAdsInfo(int adsid)
        {
            return this.dbContext.Ads.Find(adsid);
        }
        /// <summary>
        /// 获取广告详情实体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Entity.AdsDetailInfo getAdsDetailInfo(int adsdetailid)
        {
            return this.dbContext.AdsDetail.Find(adsdetailid);
        }
   
        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkAdsCode(string code,int adsid)
        {
            return this.dbContext.Ads.Count(g => g.AdsId != adsid && g.AdsCode.Equals(code)) > 0;
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="info"></param>
        public bool updateAds(Entity.AdsInfo info)
        {
            var updateInfo = this.getAdsInfo(info.AdsId);
            if (updateInfo == null)
            {
                this.dbContext.Ads.Add(info);
            }
            else
            {
                this.dbContext.Entry(updateInfo).CurrentValues.SetValues(info);
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新广告详情
        /// </summary>
        /// <param name="info"></param>
        public bool updateAdsDetail(Entity.AdsDetailInfo info)
        {
            var updateInfo = this.getAdsDetailInfo(info.AdsDetailId);
            info.EndDate = info.StartDate.AddDays(info.ShowDays);
            if (updateInfo == null)
            {
                var adsInfo = this.getAdsInfo(info.AdsCode);
                if (adsInfo != null)
                {
                    info.AdsType = adsInfo.AdsType;
                    this.dbContext.AdsDetail.Add(info);
                }
            }
            else
            {
                info.ShowNum = updateInfo.ShowNum;
                info.HitsNum = updateInfo.HitsNum;

                this.dbContext.Entry<Entity.AdsDetailInfo>(updateInfo).CurrentValues.SetValues(info);
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="code"></param>
        public void deleteAds(string code)
        {
            var info = this.getAdsInfo(code);
            if (info != null)
            {
                //删除广告详情
                this.dbContext.AdsDetail.RemoveRange(this.getAdsDetailList(code));
                //删除广告
                this.dbContext.Ads.Remove(info);

                this.dbContext.SaveChanges();
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "要删除的字典不存在！";
            }
        }

        /// <summary>
        /// 删除广告详情
        /// </summary>
        /// <param name="id"></param>
        public void deleteAdsDetails(int id)
        {
            var info = this.getAdsDetailInfo(id);
            if (info != null)
            {
                //删除key
                this.dbContext.AdsDetail.Remove(info);

                this.dbContext.SaveChanges();
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "数据不存在！";
            }
        }

    }
}
