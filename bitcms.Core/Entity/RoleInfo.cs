using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    /// <summary>
    /// bitcms_Role:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    [Table("bitcms_role")]
    public partial class RoleInfo
    {
     
        #region Model
        /// <summary>
        /// 角色Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("roleid")]
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        [Column("rolename")]
        public string RoleName { get; set; }
        /// <summary>
        /// 管理权限
        /// </summary>
        //[Column("adminpower")]
        //public string AdminPower { get; set; }
        private int readpower;
        /// <summary>
        /// 读权限
        /// </summary>
        [Column("readpower")]
        public int ReadPower
        {
            set { readpower = value; }
            get
            {
                if (readpower > 100)
                    return 100;
                else if (readpower < 0)
                    return 0;
                else
                    return readpower;
            }
        }
        /// <summary>
        /// 锁定角色
        /// </summary>
        [Column("lock")]
        public int Lock { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        [Column("roletype")]
        public string RoleType { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Column("icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 积分会员最小积分
        /// </summary>
        [Column("minscore")]
        public int MinScore { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column("orderno")]
        public int OrderNo { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        [Column("resume")]
        public string Resume { get; set; }
        #endregion Model
    }
}
