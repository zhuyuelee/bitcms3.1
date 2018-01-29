using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace bitcms.Entity
{
    [Table("bitcms_maproute")]
    public class MapRouteInfo : BasicMapRouteInfo
    {
        #region Model
        /// <summary>
        /// 重写Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("maprouteid")]
        public int MapRouteId { set; get; }
        /// <summary>
        /// 父重写Id
        /// </summary>
        [Column("fatherid")]
        public int FatherId { set; get; }

        /// <summary>
        /// 重写
        /// </summary>
        [Column("maproutename")]
        public string MapRouteName { set; get; }

        /// <summary>
        /// 启用
        /// </summary>
        [Column("enabled")]
        public int Enabled { set; get; }
        #endregion Model
    }

    [Serializable]
    [XmlType("Item")]
    public class BasicMapRouteInfo
    {
        /// <summary>
        /// 重写地址
        /// </summary>
        [Column("lookfor")]
        public string LookFor { set; get; }
        /// <summary>
        /// 转向地址
        /// </summary>
        [Column("sendto")]
        public string SendTo { set; get; }
        /// <summary>
        /// Template
        /// </summary>
        [Column("template")]
        public string Template { set; get; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        [Column("cachetime")]
        public int CacheTime{set; get;}
    }
}
