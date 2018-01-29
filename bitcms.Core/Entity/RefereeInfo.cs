using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    /// <summary>
    /// bitcms_referee
    /// </summary>
    [Table("bitcms_referee")]
    public class RefereeInfo
    {
        /// <summary>
        /// UserId
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("userid",Order=1)]
        public int UserId { get; set; }
        /// <summary>
        /// 推荐人id
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("refereeid", Order = 2)]
        public int RefereeId { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [Column("referee")]
        public string Referee { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("indate")]
        public DateTime InDate { get; set; }
    }
}
