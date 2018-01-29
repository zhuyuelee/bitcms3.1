using System;
using System.Collections.Generic;
using System.Linq;
using bitcms.Common;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public int  checkItemCode(string itemcode, int itemid)
        {
            return this.dbContext.Item.Count(g => g.ItemCode.Equals(itemcode) && g.ItemId != itemid);
        }

        /// <summary>
        /// 获取项目Id
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public List<Entity.ItemInfo> getItemList(string code, int fatherid)
        {
            return this.dbContext.Item.Where(g => g.FatherId == fatherid && g.ChannelCode == code).OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 获取项目Id
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public List<Entity.ItemInfo> getItemList(string code)
        {
            return this.dbContext.Item.Where(g => g.ChannelCode == code).OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 获取项目Id
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public List<Entity.ItemInfo> getItemList(string code, int fahterid, int show)
        {
            var lambda = PredicateExtensions.True<Entity.ItemInfo>();
            lambda = lambda.And(g => g.ChannelCode == code);
            if (fahterid > -1)
            {
                lambda = lambda.And(g => g.FatherId == fahterid);
            }
            if (show > -1)
            {
                lambda = lambda.And(g => (g.Show & show) == show);
            }

            return this.dbContext.Item.Where(lambda).OrderBy(g => g.OrderNo).ToList();
        }
        /// <summary>
        /// 获取项目实体
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public Entity.ItemInfo getItemInfo(string itemcode)
        {
            return this.dbContext.Item.FirstOrDefault(g => g.ItemCode.Equals(itemcode));
        }
        /// <summary>
        /// 获取项目实体
        /// </summary>
        /// <param name="fatherid"></param>
        /// <returns></returns>
        public Entity.ItemInfo getItemInfo(int itemid)
        {
            return this.dbContext.Item.Find(itemid);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public bool deleteItem(int itemid)
        {
            if (this.dbContext.Item.Count(g => g.FatherId == itemid) == 0 && this.dbContext.Detail.Count(g => g.ItemId == itemid) == 0)
            {
                var itemInfo = getItemInfo(itemid);

                if (itemInfo != null)
                {
                    this.dbContext.Item.Remove(itemInfo);
                    return this.dbContext.SaveChanges() > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新栏目
        /// </summary>
        /// <param name="info"></param>
        public bool updateItem(Entity.ItemInfo info)
        {
            if (string.IsNullOrEmpty(info.ItemCode))
            { 
                do
                {
                    info.ItemCode = Guid.NewGuid().ToString("N");
                } while (checkItemCode(info.ItemCode, info.ItemId) > 0);
            }

            if (info.ItemId <= 0)
            {
                this.dbContext.Item.Add(info);
            }
            else
            {
                this.dbContext.Entry(info).State = System.Data.Entity.EntityState.Modified;
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 获取栏目阅读权限
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public int getItemReadPower(int itemid)
        {
            var power = 0;
            do{
                var iteminfo = this.getItemInfo(itemid);
                power = iteminfo.ReadPower;
                if (power > 0)
                {
                    break;
                }
                else
                {
                    itemid = iteminfo.FatherId;
                }
                
            } while (itemid > 0);

            return power;
        }
    }
}
