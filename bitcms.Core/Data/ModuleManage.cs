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
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.ModuleInfo> getModuleList()
        {
            return this.dbContext.Module.ToList();
        }
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        public List<Entity.ModuleInfo> getModuleList(Entity.ModuleType type)
        {
            return this.dbContext.Module.Where(g => g.ModuleType == type).ToList();
        }
        /// <summary>
        /// 获取模块实体
        /// </summary>
        /// <returns></returns>
        public Entity.ModuleInfo getModuleInfo(string code)
        {
            return this.dbContext.Module.FirstOrDefault(g => g.ModuleCode == code && g.Enabled == 1);
        }
        /// <summary>
        /// 根据AppId获取模块实体
        /// </summary>
        /// <returns></returns>
        public Entity.ModuleInfo getModuleInfo(string appid, Entity.ModuleType type)
        {
            return this.dbContext.Module.FirstOrDefault(g => g.ModuleType == type && g.AppId.ToLower() == appid && g.Enabled == 1);
        }
        /// <summary>
        /// 获取模块实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.ModuleInfo getModuleInfo(int moduleid)
        {
            return this.dbContext.Module.Find(moduleid);
        }
        /// <summary>
        /// 检查模块编码
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkModuleCode(string modulecode, int moduleid)
        {
            var lambda = PredicateExtensions.True<Entity.ModuleInfo>();
            lambda = lambda.And(g => g.ModuleCode.Equals(modulecode) && g.ModuleId != moduleid);

            return this.dbContext.Module.Count(lambda) <= 0;
        }

        /// <summary>
        /// 检查模块AppId编码
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool checkModuleAppId(string appid, int moduleid)
        {
            var lambda = PredicateExtensions.True<Entity.ModuleInfo>();
            lambda = lambda.And(g => g.AppId.ToLower().Equals(appid) && g.ModuleId != moduleid);
            return this.dbContext.Module.Count(lambda) <= 0;
        }

        /// <summary>
        /// 更新模块
        /// </summary>
        /// <param name="info"></param>
        public bool updateModule(Entity.ModuleInfo info)
        {

            if (string.IsNullOrEmpty(info.AppId))
            {
                info.AppId = Utils.random(16, false).ToLower();
            }
            if (string.IsNullOrEmpty(info.AppSecret))
            {
                info.AppSecret = Guid.NewGuid().ToString("N");
            }

            var moduleInfo = getModuleInfo(info.ModuleId);
            if (moduleInfo == null)
            {
                info.InDate = Config.SiteConfig.getLocalTime();
                this.dbContext.Module.Add(info);
            }
            else
            {
                info.InDate = moduleInfo.InDate;
                this.dbContext.Entry<Entity.ModuleInfo>(moduleInfo).CurrentValues.SetValues(info);

            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="list"></param>
        public bool deleteModule(List<Entity.ModuleInfo> list)
        {
            this.dbContext.Module.RemoveRange(list);
            return this.dbContext.SaveChanges() > 0;
        }
    }
}