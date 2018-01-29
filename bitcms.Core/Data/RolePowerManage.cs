using System.Collections.Generic;
using System.Linq;
using bitcms.DataProvider;

namespace bitcms.Data
{
    public partial class CMSManage : DataBase
    {
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public List<Entity.RolePowerInfo> getRolePowerList(int roleid)
        {
            return this.dbContext.RolePower.Where(g => g.RoleId == roleid).ToList();
        }

        /// <summary>
        /// 获取角色权限实体
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public Entity.RolePowerInfo getRolePowerInfo(int roleid, int menuid)
        {
            return this.dbContext.RolePower.Find(roleid, menuid);
        }
    }
}
