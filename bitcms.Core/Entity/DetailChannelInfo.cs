using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_detailchannel")]
    [Serializable]
    public class DetailChannelInfo
    {
        private string channelcode = "";
        /// <summary>
        /// 频道编码
        /// </summary>		
        [Key, Column("channelcode", Order = 1)]
        public string ChannelCode
        {
            get
            {
                if (this.channelcode == null)
                {
                    this.channelcode = "";
                }
                return this.channelcode.ToLower();
            }
            set
            {
                this.channelcode = value;
            }
        }
        /// <summary>
        /// 频道名
        /// </summary>		
        [Column("channelname")]
        public string ChannelName { get; set; }
        /// <summary>
        /// 前台路径
        /// </summary>		
        [Column("path")]
        public string Path { get; set; }
        private string detailpath = "";
        /// <summary>
        /// 页面预览路径
        /// </summary>		
        [Column("detailpath")]
        public string DetailPath { get { return this.detailpath == null ? "" : this.detailpath; } set { this.detailpath = value; } }

        /// <summary>
        /// 启用
        /// </summary>		
        [Column("enabled")]
        public int Enabled { get; set; }
        /// <summary>
        /// 内容编码唯一
        /// </summary>		
        [Column("enableddetailcode")]
        public int EnabledDetailCode { get; set; }
        /// <summary>
        /// 阅读权限
        /// </summary>		
        [Column("enabledreadpower")]
        public int EnabledReadPower { get; set; }
        /// <summary>
        /// 启用内容分页
        /// </summary>		
        [Column("enabledpaging")]
        public int EnabledPaging { get; set; }
        /// <summary>
        /// 推荐设置
        /// </summary>		
        [Column("shows")]
        public string Shows { get; set; }
        /// <summary>
        /// 栏目深度
        /// </summary>		
        [Column("itemdeep")]
        public int ItemDeep { get; set; }
        /// <summary>
        /// 封面设置
        /// </summary>		
        [Column("coverset")]
        public int CoverSet { get; set; }
        /// <summary>
        /// 音频设置
        /// </summary>		
        [Column("audioset")]
        public int AudioSet { get; set; }
        /// <summary>
        /// 视频设置
        /// </summary>		
        [Column("videoset")]
        public int VideoSet { get; set; }
        /// <summary>
        /// 附件设置
        /// </summary>		
        [Column("attachmentset")]
        public int AttachmentSet { get; set; }
        /// <summary>
        /// 图库设置
        /// </summary>		
        [Column("galleryset")]
        public GallerySet GallerySet { get; set; }

        /// <summary>
        /// 图库标签
        /// </summary>		
        [Column("gallerytags")]
        public string GalleryTags { get; set; }
        /// <summary>
        /// 图库宽度限制
        /// </summary>		
        [Column("width")]
        public int Width { get; set; }
        /// <summary>
        /// 图库高度限制
        /// </summary>		
        [Column("height")]
        public int Height { get; set; }
        /// <summary>
        /// 排序
        /// </summary>		
        [Column("orderno")]
        public int OrderNo { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>		
        [Column("keywords")]
        public string Keywords { get; set; }
        /// <summary>
        /// 概要介绍
        /// </summary>		
        [Column("resume")]
        public string Resume { get; set; }
    }
}
