using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_follow")]
    [Serializable]
    public class FollowInfo
    {
        /// <summary>
        /// 会员id
        /// </summary>		
        [Column("userid", Order = 1)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        /// <summary>
        /// 被关注对象
        /// </summary>
        [Column("followuserid", Order = 2)]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FollowUserId { get; set; }
        /// <summary>
        /// 相互关注
        /// </summary>
        [Column("mutual")]
        public int Mutual { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }
    }
}
