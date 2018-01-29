using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    /// <summary>
    /// 后台管理菜单类
    /// </summary>
    [Table("bitcms_adminmenu")]
    public class AdminMenuInfo
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adminmenuid")]
        public int AdminMenuId { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        [Column("menuname")]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单父Id
        /// </summary>
        [Column("fatherid")]
        public int FatherId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("orderno")]
        public int OrderNo { get; set; }

        private string area = "";
        /// <summary>
        /// Area区域
        /// </summary>
        [Column("area")]
        public string Area
        {
            get
            {
                if (this.area == null)
                {
                    this.area = "";
                }
                return this.area.ToLower();
            }
            set
            {
                this.area = value;
            }
        }

        private string controller = "";
        /// <summary>
        /// 控制器
        /// </summary>
        [Column("controller")]
        public string Controller
        {
            get
            {
                if (this.controller == null)
                {
                    this.controller = "";
                }
                return this.controller.ToLower();
            }
            set
            {
                this.controller = value;
            }
        }

        private string parm = "";
        /// <summary>
        /// 参数
        /// </summary>
        [Column("parm")]
        public string Parm
        {
            get
            {
                if (this.parm == null)
                {
                    this.parm = "";
                }
                return this.parm.ToLower();
            }
            set
            {
                this.parm = value;
            }
        }
        /// <summary>
        /// 说明
        /// </summary>
        [Column("explain")]
        public string Explain { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        [Column("roletype")]
        public string RoleType { get; set; }

        /// <summary>
        /// 显示菜单
        /// </summary>
        [Column("display")]
        public int Display { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column("icon")]
        public string Icon { get; set; }
    }
}
