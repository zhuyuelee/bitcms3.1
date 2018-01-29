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
        /// 获取角色实体
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Entity.RoleInfo getRoleInfo(int roleid)
        {
            return this.dbContext.Role.Find(roleid);
        }
        /// <summary>
        /// 根据角色类型获取最小阅读权限角色实体
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Entity.RoleInfo getRoleByScore(string roletype, int score)
        {
            var lambda = PredicateExtensions.True<Entity.RoleInfo>();
            lambda = lambda.And(g => g.RoleType == roletype && g.Lock == 0);
            if (score > 0)
            {
                lambda = lambda.And(g => g.MinScore <= score);
                return this.dbContext.Role.Where(lambda).OrderByDescending(g => g.MinScore).FirstOrDefault();
            }
            else
            {
                return this.dbContext.Role.Where(lambda).OrderBy(g => g.MinScore).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据角色类型获取最小阅读权限角色实体
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Entity.RoleInfo getRoleByReadPower(string roletype, int readpower)
        {
            var lambda = PredicateExtensions.True<Entity.RoleInfo>();
            lambda = lambda.And(g => g.RoleType == roletype && g.Lock == 0);
            if (readpower > 0)
            {
                lambda = lambda.And(g => g.ReadPower >= readpower);
            }
            return this.dbContext.Role.Where(lambda).OrderBy(g => g.ReadPower).FirstOrDefault();
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        public List<Entity.RoleInfo> getRoleList()
        {
            return this.dbContext.Role.OrderBy(g => g.RoleType).OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        public List<Entity.RoleInfo> getRoleEnabledList(string roletype = null)
        {
            var lambda = PredicateExtensions.True<Entity.RoleInfo>();
            lambda = lambda.And(g => g.Lock == 0);
            if (!string.IsNullOrEmpty(roletype))
            {
                lambda = lambda.And(g => g.RoleType.Equals(roletype));
            }
            return this.dbContext.Role.Where(lambda).OrderBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        public bool updateRoleInfo(Entity.RoleInfo info, List<Entity.RolePowerInfo> powerList)
        {
            if (info.RoleId <= 0)
            {
                this.dbContext.Role.Add(info);
            }
            else
            {
                this.dbContext.Entry(info).State = System.Data.Entity.EntityState.Modified;
            }
            if (this.dbContext.SaveChanges() > 0)
            {
                if (powerList.Count > 0)
                {//更新权限
                    this.updateRolePower(info.RoleId, powerList);
                    return this.Error == 0;
                }
            }
            return true;
        }

        /// <summary>
        /// 批量更新权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="powerList"></param>
        public void updateRolePower(int roleid, List<Entity.RolePowerInfo> powerList)
        {
            try
            {
                var menuIds = new List<int>();
                var newPowers = new List<Entity.RolePowerInfo>();
                foreach (var power in powerList)
                {
                    if (power.RoleId > 0)
                    {
                        menuIds.Add(power.AdminMenuId);
                        var info = dbContext.RolePower.Find(power.RoleId, power.AdminMenuId);
                        if (info != null)
                        {
                            info.Read = power.Read;
                            info.Edit = power.Edit;
                        }
                        else
                        {
                            dbContext.RolePower.Add(power);
                        }
                    }
                    else
                    {
                        power.RoleId = roleid;
                        dbContext.RolePower.Add(power);
                    }

                };
                //删除原来原权限
                var delList = this.dbContext.RolePower.Where(g => g.RoleId == roleid && !menuIds.Contains(g.AdminMenuId));
                if (delList.Count() > 0)
                {
                    this.dbContext.RolePower.RemoveRange(delList);
                }
                dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Error = Entity.Error.错误;
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        public void deleteRoles(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string[] arrIds = ids.Split(',');
                var delList = new List<Entity.RoleInfo>();
                foreach (string strId in arrIds)
                {
                    var id = Utils.strToInt(strId);
                    //包括会员
                    if (this.dbContext.User.Count(g => g.RoleId == id) > 0)
                    {
                        this.Error++;
                    }
                    else
                    {
                        delList.Add(this.getRoleInfo(id));
                    }
                }
                if (delList.Count > 0)
                {

                    foreach (var info in delList)
                    {
                        this.dbContext.AdminMenuShortcut.RemoveRange(this.dbContext.AdminMenuShortcut.Where(g => g.RoleId == info.RoleId));
                        this.dbContext.AdminMenuShortcut.RemoveRange(this.dbContext.AdminMenuShortcut.Where(g => g.RoleId == info.RoleId));
                        this.dbContext.RolePower.RemoveRange(this.dbContext.RolePower.Where(g => g.RoleId == info.RoleId));
                    }

                    this.dbContext.Role.RemoveRange(delList);
                    this.dbContext.SaveChanges();
                }
            }
        }
    }
}

