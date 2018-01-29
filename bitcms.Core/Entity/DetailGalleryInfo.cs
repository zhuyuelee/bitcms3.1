using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_detailgallery")]
    [Serializable]
    public class DetailGalleryInfo
    {
        /// <summary>
        /// 图库id
        /// </summary>		
        [Column("galleryid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GalleryId { get; set; }
       
        /// <summary>
        /// 内容id
        /// </summary>		
        [Column("detailid")]
        public int DetailId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>		
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 阅读权限值
        /// </summary>		
        [Column("readpower")]
        public int ReadPower { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>		
        [Column("agreenum")]
        public int AgreeNum { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>		
        [Column("follownum")]
        public int FollowNum { get; set; }
        /// <summary>
        /// 查看数
        /// </summary>		
        [Column("hitsnum")]
        public int HitsNum { get; set; }
        /// <summary>
        /// 地址
        /// </summary>		
        [Column("path")]
        public string Path { get; set; }
        /// <summary>
        /// 概要介绍
        /// </summary>		
        [Column("resume")]
        public string Resume { get; set; }
        /// <summary>
        /// 图库标签
        /// </summary>		
        [Column("tag")]
        public string Tag { get; set; }
        /// <summary>
        /// 排序
        /// </summary>		
        [Column("orderno")]
        public int OrderNo { get; set; }
        /// <summary>
        /// 封面
        /// </summary>		
        [Column("cover")]
        public int Cover { get; set; }
        /// <summary>
        /// 类型
        /// </summary>		
        [Column("gallerytype")]
        public GalleryType GalleryType { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }        
    }
}
