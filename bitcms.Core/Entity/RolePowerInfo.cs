using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Table("bitcms_rolepower")]
    public class RolePowerInfo
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("roleid", Order = 1)]
        public int RoleId { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("adminmenuid",Order = 2)]
        public int AdminMenuId { get; set; }

        /// <summary>
        /// 读权限
        /// </summary>
        [Column("read")]
        public int Read { get; set; }
        /// <summary>
        /// 写权限
        /// </summary>
        [Column("edit")]
        public int Edit { get; set; }
    }
}
