using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {

        /// <summary>
        /// 更新城市
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool deleteCity(int cityid)
        {
            var cityInfo = this.getCityInfo(cityid);
            if (cityInfo != null)
            {
                    var list = new List<Entity.CityInfo>();
                if (cityInfo.Deep == 0)
                {
                    list.AddRange(this.getCityList(cityInfo.CityId));//城市
                    list.ForEach(g => {
                        this.dbContext.City.RemoveRange(this.getCityList(g.CityId));//区县
                    });
                }
                else if (cityInfo.Deep == 1)
                {
                    list.AddRange(this.getCityList(cityInfo.CityId));//区县
                }
                if (list.Count > 0)
                {
                    list.AddRange(list);
                }
                this.dbContext.City.Remove(cityInfo);

            }
           
            return this.dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 更新城市
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool updateCityInfo(Entity.CityInfo info)
        {
            var cityInfo = this.getCityInfo(info.CityId);
            if (cityInfo != null)
            {
                this.dbContext.Entry(cityInfo).CurrentValues.SetValues(info);
            }
            else
            {
                this.dbContext.City.Add(info);
            }
            return this.dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public Entity.CityInfo getCityInfo(int cityid)
        {
            return this.dbContext.City.Find(cityid);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public List<Entity.CityInfo> getCityList(int cityid)
        {
            return this.dbContext.City.Where(g => g.FatherCityId == cityid).OrderBy(g => g.OrderNo).ThenBy(g => g.CityId).ToList();
        }
        /// <summary>
        /// 获取推荐城市列表
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public List<Entity.CityInfo> getCityList(int cityid, int show, int deep = -1)
        {
            var lambda = PredicateExtensions.True<Entity.CityInfo>();
            if (cityid > -1)
            {
                lambda = lambda.And(g => g.FatherCityId == cityid);
            }
            if (deep > -1)
            {
                lambda = lambda.And(g => g.Deep == deep);
            }
            if (show == 1)
            {
                lambda = lambda.And(g => g.Show == 1);
            }
            return this.dbContext.City.Where(lambda).OrderBy(g => g.OrderNo).ThenBy(g => g.CityId).ToList();
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public List<Entity.CityInfo> getProvinceList(string diliquhua)
        {
            return this.dbContext.City.Where(g => g.DiliQuhua == diliquhua && g.FatherCityId == 0).OrderBy(g => g.OrderNo).ThenBy(g => g.CityId).ToList();
        }
    }
}
