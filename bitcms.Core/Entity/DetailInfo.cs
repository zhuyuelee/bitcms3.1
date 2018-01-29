using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_detail")]
    [Serializable]
    public class DetailInfo 
    {
        /// <summary>
        /// 内容id
        /// </summary>		
        [Column("detailid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        /// <summary>
        /// 栏目id
        /// </summary>		
        [Column("itemid")]
        public int ItemId { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>		
        [Column("userid")]
        public int UserId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>		
        [Column("detailcode")]
        public string DetailCode { get; set; }
        /// <summary>
        /// 频道编码
        /// </summary>		
        [Column("channelcode")]
        public string ChannelCode { get; set; }
        /// <summary>
        /// 阅读权限值
        /// </summary>		
        [Column("readpower")]
        public int ReadPower { get; set; }
        /// <summary>
        /// 分类
        /// </summary>		
        [Column("items")]
        public string Items { get; set; }
        /// <summary>
        /// 标题
        /// </summary>		
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>		
        [Column("subtitle")]
        public string SubTitle { get; set; }
        /// <summary>
        /// 展示
        /// </summary>		
        [Column("display")]
        public int Display { get; set; }
        /// <summary>
        /// 展示
        /// </summary>		
        [Column("displaytime")]
        public DateTime DisplayTime { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>		
        [Column("show")]
        public int Show { get; set; }

        private decimal stars = 1;
        /// <summary>
        /// 评级  
        /// </summary>		
        [Column("stars")]
        public decimal Stars
        {
            get { return this.stars; }
            set { this.stars = value; }
        }
        /// <summary>
        /// 概要介绍
        /// </summary>		
        [Column("resume")]
        public string Resume { get; set; }
        /// <summary>
        /// 查找关键字
        /// </summary>		
        [Column("searchkey")]
        public string SearchKey { get; set; }
        /// <summary>
        /// 查找关键字
        /// </summary>		
        [Column("keyword")]
        public string Keyword { get; set; }
        /// <summary>
        /// 封面
        /// </summary>		
        [Column("galleryid")]
        public int GalleryId { get; set; }
        /// <summary>
        /// 图库统计 格式：cover:3;picture:10
        /// </summary>
        [Column("gallerystatistics")]
        public string GalleryStatistics { get; set; }
        /// <summary>
        /// 关闭评论
        /// </summary>		
        [Column("closereview")]
        public int CloseReview { get; set; }
        /// <summary>
        /// 启用外链
        /// </summary>		
        [Column("enabledlink")]
        public int EnabledLink { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>		
        [Column("link")]
        public string Link { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>		
        [Column("agreenum")]
        public int AgreeNum { get; set; }
        /// <summary>
        /// 反对数
        /// </summary>		
        [Column("againstnum")]
        public int AgainstNum { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>		
        [Column("follownum")]
        public int FollowNum { get; set; }
        /// <summary>
        /// 查看次数
        /// </summary>		
        [Column("hitsnum")]
        public int HitsNum { get; set; }
        /// <summary>
        /// 回复次数
        /// </summary>		
        [Column("reviewnum")]
        public int ReviewNum { get; set; }
        /// <summary>
        /// recycle
        /// </summary>		
        [Column("recycle")]
        public int Recycle { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>		
        [Column("recycledate")]
        public DateTime? RecycleDate { get; set; }
        /// <summary>
        /// 作者
        /// </summary>		
        [Column("author")]
        public string Author { get; set; }
        /// <summary>
        /// 出处
        /// </summary>		
        [Column("source")]
        public string Source { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }

    }

    [Table("bitcms_detailcontent")]
    [Serializable]
    public class DetailContentInfo
    {
        /// <summary>
        /// 内容详情id
        /// </summary>		
        [Column("contentid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentId { get; set; }
        /// <summary>
        /// 内容id
        /// </summary>		
        [Column("detailid")]
        public int DetailId { get; set; }
        /// <summary>
        /// 栏目id
        /// </summary>		
        [Column("itemid")]
        public int ItemId { get; set; }
        /// <summary>
        /// 频道编码
        /// </summary>		
        [Column("channelcode")]
        public string ChannelCode { get; set; }
        /// <summary>
        /// 标题
        /// </summary>		
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 排序
        /// </summary>		
        [Column("orderno")]
        public int OrderNo { get; set; }
        /// <summary>
        /// 内容
        /// </summary>		
        [Column("content")]
        public string Content { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }
    }
        [NotMapped]
    public class UserDetailInfo : DetailInfo
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

        /// <summary>
        /// ItemName
        /// </summary>		
        [Column("itemname")]
        public string ItemName { get; set; }
        /// <summary>
        /// 图标
        /// </summary>		
        [Column("icon")]
        public string Icon { get; set; }
    }

}
