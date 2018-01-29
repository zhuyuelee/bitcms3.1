using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取菜单实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.AdminMenuInfo getAdminMenuInfo(int id)
        {
            return dbContext.AdminMenu.Find(id);
        }

        /// <summary>
        /// 获取菜单实体
        /// </summary>
        /// <param name="_area"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuInfo> getAdminMenuList(string area, string controller)
        {
            return dbContext.AdminMenu.Where(g => g.Area == area && g.Controller == controller).ToList();
        }

        /// <summary>
        /// 检查控制器是否已经存在
        /// </summary>
        /// <param name="checkController"></param>
        /// <param name="area"></param>
        /// <param name="adminmenuid"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public bool checkController(string controller, string area, int adminmenuid, string parm)
        {
            var lambda = PredicateExtensions.True<Entity.AdminMenuInfo>();
            lambda = lambda.And(o => o.Area.Equals(area) && o.Controller.Equals(controller) && o.AdminMenuId != adminmenuid);

            if (!string.IsNullOrEmpty(parm))
            {
                lambda = lambda.And(o => o.Parm.Equals(parm));
            }
            else
            {
                lambda = lambda.And(o => o.Parm == null);
            }

            return this.dbContext.AdminMenu.Count(lambda) > 0;
            
        }

        /// <summary>
        /// 根据父类Id获取子菜单
        /// </summary>
        /// <param name="fatherId"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuInfo> getAdminMenuList(int display)
        {
            return this.getAdminMenuList(-1, null, display);
        }

        /// <summary>
        /// 根据父类Id获取子菜单
        /// </summary>
        /// <param name="fatherId"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuInfo> getAdminMenuList(int fatherId, string type)
        {
            return this.getAdminMenuList(fatherId, type, 0);
        }

        /// <summary>
        /// 根据父类Id获取子菜单
        /// </summary>
        /// <param name="fatherId"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuInfo> getAdminMenuList(int fatherId, string type, int display)
        {
            var lambda = PredicateExtensions.True<Entity.AdminMenuInfo>();
            if (fatherId > -1)
            {
                lambda = lambda.And(o => o.FatherId == fatherId);
            }

            if(!string.IsNullOrEmpty(type))
            {
                lambda = lambda.And(o => o.RoleType == type);
            }
            if (display == 1)
            {
                lambda = lambda.And(o => o.Display == 1);
            }

            return dbContext.AdminMenu.Where(lambda).OrderBy(g => g.RoleType).ThenBy(g => g.FatherId).ThenBy(g => g.OrderNo).ToList();
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuInfo> getPowerList(int roleid)
        {
            return (from s in this.dbContext.RolePower
                    join m in this.dbContext.AdminMenu
                     on s.AdminMenuId equals m.AdminMenuId
                    where s.RoleId == roleid
                    select m).ToList();
        }

        /// <summary>
        /// 根据父id获取菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="RolePower"></param>
        /// <param name="userid"></param>
        /// <param name="shortCat"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuInfo> getPowerList(int fatherid, int roleid, int userid, int shortcut)
        {
            if (shortcut == 1 && userid == 1)
            {
                return (from s in this.dbContext.AdminMenuShortcut
                        join m in this.dbContext.AdminMenu
                         on s.AdminMenuId equals m.AdminMenuId
                        where s.UserId == userid && s.RoleId == roleid
                        orderby m.OrderNo
                        select m).ToList();
            }

            var lambda = PredicateExtensions.True<Entity.AdminMenuInfo>();
            var roleInfo = this.dbContext.Role.Find(roleid);
            lambda = lambda.And(o => o.Display == 1 && o.RoleType == roleInfo.RoleType);
            if (shortcut == 0)
            {
                lambda = lambda.And(o => o.FatherId == fatherid);
            }

            List<Entity.RolePowerInfo> powerList = null;
            var menus = this.dbContext.AdminMenu.Where(lambda);
            if (shortcut == 1)
            {
                powerList = (from s in this.dbContext.AdminMenuShortcut
                             join p in this.dbContext.RolePower on s.AdminMenuId equals p.AdminMenuId
                             where s.UserId == userid && p.RoleId == roleid
                             select p).ToList();
            }


            if (userid > 1 && powerList != null)
            {
                menus = from m in menus
                        join p in powerList on m.AdminMenuId equals p.AdminMenuId
                        select m;
            }
            return menus.OrderBy(g => g.OrderNo).ToList();
        }
        /// <summary>
        /// 获取快捷菜单
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<Entity.AdminMenuShortcutInfo> getsAdminMenuShortcutList(int roleid, int userid)
        {
            return this.dbContext.AdminMenuShortcut.Where(g => g.UserId == userid && g.RoleId == roleid).ToList();
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>

        public bool updateAdminMenu(Entity.AdminMenuInfo info)
        {
            if (info.AdminMenuId <= 0)
            {
                this.dbContext.AdminMenu.Add(info);
            }
            else
            {
                this.dbContext.Entry(info).State = System.Data.Entity.EntityState.Modified;
            }
            return this.dbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void deleteAdminMenu(int id)
        {
            var info = this.dbContext.AdminMenu.Find(id);
            var list = this.getAdminMenuList(id, info.RoleType);
            if (list.Count == 0)
            {
                this.dbContext.AdminMenu.Remove(info);
                this.dbContext.SaveChanges();
            }
            else
            {
                this.Error = Entity.Error.错误;
                this.Message = "该菜单包含子菜单";
            }
        }

        /// <summary>
        /// 更新快捷菜单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool updateAdminMenuShortcut(List<Entity.AdminMenuShortcutInfo> list, int roleid, int userid)
        {
            var oldlist = this.getsAdminMenuShortcutList(roleid, userid);
            var dellist = new List<Entity.AdminMenuShortcutInfo>();
            oldlist.ForEach(g =>
            {
                if (list.Count(m => m.RoleId == g.RoleId && m.AdminMenuId == g.AdminMenuId && g.UserId == g.UserId) == 0)
                {
                    dellist.Add(g);
                }
                else
                {
                    list.RemoveAll(m => m.RoleId == g.RoleId && m.AdminMenuId == g.AdminMenuId && g.UserId == g.UserId);
                }
            });

            if (dellist.Count > 0)
            {
                this.dbContext.AdminMenuShortcut.RemoveRange(dellist);
            }
            if (list.Count > 0)
            {
                this.dbContext.AdminMenuShortcut.AddRange(list);
            }

            return this.dbContext.SaveChanges() > 0;
        }
    }
}
