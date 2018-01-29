using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Serializable]
    [Table("bitcms_review")]
    public class ReviewInfo : BaseReviewInfo { }

    [Serializable]
    [Table("bitcms_userreviewview")]
    public class UserReviewInfo : BaseReviewInfo
    {
        /// <summary>
        /// UserName
        /// </summary>		
        [Column("username")]
        public string UserName { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>		
        [Column("avatar")]
        public string Avatar { get; set; }
    }

    [Serializable]
    [Table("bitcms_detailreviewview")]
    public class DetailReviewInfo : BaseReviewInfo
    {
        /// <summary>
        /// 标题
        /// </summary>		
        [Column("detailtitle")]
        public string DetailTitle { get; set; }
        /// <summary>
        /// 概要介绍
        /// </summary>		
        [Column("resume")]
        public string Resume { get; set; }

        /// <summary>
        /// 封面
        /// </summary>		
        [Column("galleryid")]
        public string GalleryId { get; set; }
    }

    /// <summary>
    /// 评论
    /// </summary>
    [Serializable]
    public class BaseReviewInfo
    {
        /// <summary>
        /// 评论id
        /// </summary>		
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("reviewid")]
        public int ReviewId { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>		
        [Column("userid")]
        public int UserId { get; set; }
        /// <summary>
        /// 内容id
        /// </summary>		
        [Column("detailid")]
        public int DetailId { get; set; }
        /// <summary>
        /// 评论id
        /// </summary>		
        [Column("replyid")]
        public int ReplyId { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>		
        [Column("show")]
        public int Show { get; set; }
        /// <summary>
        /// 频道编码
        /// </summary>		
        [Column("channelcode")]
        public string ChannelCode { get; set; }

        /// <summary>
        /// 标签
        /// </summary>		
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>		
        [Column("content")]
        public string Content { get; set; }
        /// <summary>
        /// 审核
        /// </summary>		
        [Column("verify")]
        public int Verify { get; set; }
        /// <summary>
        /// 保密评论
        /// </summary>		
        [Column("secrecy")]
        public int Secrecy { get; set; }
        /// <summary>
        /// 反对数
        /// </summary>		
        [Column("againstnum")]
        public int AgainstNum { get; set; }
        /// <summary>
        /// 赞同数
        /// </summary>		
        [Column("agreenum")]
        public int AgreeNum { get; set; }

        /// <summary>
        /// 回复数
        /// </summary>
        [Column("replynum")]
        public int ReplyNum { get; set; }
        /// <summary>
        /// 查看数
        /// </summary>
        [Column("hitsnum")]
        public int HitsNum { get; set; }
        /// <summary>
        /// 评语日期
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }

    }

}
