using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_detailhits")]
    [Serializable]
    public class DetailHitsInfo
    {
        /// <summary>
        /// 点击编码
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("detailhitscode")]
        public string DetailHitsCode { get; set; }
        /// <summary>
        /// 会员在线编码
        /// </summary>		
        [Column("useronlineid")]
        public string UserOnlineId { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>		
        public int UserId { get; set; }
        /// <summary>
        /// 内容id
        /// </summary>		
        public int DetailId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }
    }
}
