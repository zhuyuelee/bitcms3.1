using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_module")]
    [Serializable]
    public class ModuleInfo
    {
        /// <summary>
        /// 模块id
        /// </summary>		
        [Column("moduleid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
       
        /// <summary>
        /// 模块名
        /// </summary>		
        [Column("modulename")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 模块类型
        /// </summary>		
        [Column("moduletype")]
        public ModuleType ModuleType { get; set; }

        private string modulecode;
        /// <summary>
        /// 模块编码
        /// </summary>		
        [Column("modulecode")]
        public string ModuleCode
        {
            get {
                if (this.modulecode == null)
                {
                    this.modulecode = "";
                }
                return this.modulecode.ToLower();
            }
            set { this.modulecode = value; }
        }
        /// <summary>
        /// AppId
        /// </summary>		
        [Column("appid")]
        public string AppId { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>		
        [Column("appsecret")]
        public string AppSecret { get; set; }
        /// <summary>
        /// 扩展字段名称
        /// </summary>
        [Column("extendname")]
        public string ExtendName { get; set; }
        /// <summary>
        /// 扩展字段1
        /// </summary>
        [Column("extend1")]
        public string Extend1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        [Column("extend2")]
        public string Extend2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        [Column("extend3")]
        public string Extend3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        [Column("extend4")]
        public string Extend4 { get; set; }
        /// <summary>
        /// 启用
        /// </summary>		
        [Column("enabled")]
        public int Enabled { get; set; }
        /// <summary>
        /// 时间戳过期时间
        /// </summary>		
        [Column("timestampexpired")]
        public int TimestampExpired { get; set; }
        /// <summary>
        /// 说明
        /// </summary>		
        [Column("explain")]
        public string Explain { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }       
    }
}
