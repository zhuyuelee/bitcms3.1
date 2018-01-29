using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_viewpoint")]
    [Serializable]
    public class ViewPointInfo
    {
        /// <summary>
        /// 会员Id
        /// </summary>		
        [Column("userid", Order = 1)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        /// <summary>
        /// 内容id
        /// </summary>		
        [Column("detailid", Order = 2)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetailId { get; set; }
        /// <summary>
        /// 评论id
        /// </summary>		
        [Column("reviewid", Order = 3)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReviewId { get; set; }
        /// <summary>
        /// 反对
        /// </summary>		
        [Column("against")]
        public int Against { get; set; }
        /// <summary>
        /// 赞同
        /// </summary>		
        [Column("agree")]
        public int Agree { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }
    }
}
