using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_adminmenushortcut")]
    [Serializable]
    public class AdminMenuShortcutInfo
    {

        /// <summary>
        /// 管理菜单Id
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("adminmenuid", Order = 1)]
        public int AdminMenuId { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("userid", Order = 2)]
        public int UserId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("roleid", Order = 3)]
        public int RoleId { get; set; }

    }
}
