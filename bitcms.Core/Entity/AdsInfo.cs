using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_ads")]
    public class AdsInfo
    {
        #region Model
        /// <summary>
        /// 广告id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adsid")]
        public int AdsId { get; set; }

        private string adscode = "";
        /// <summary>
        /// 广告编码
        /// </summary>
        [Column("adscode")]
        public string AdsCode
        {
            get
            {
                if (this.adscode == null)
                {
                    this.adscode = "";
                }
                return this.adscode.ToLower();
            }
            set
            {
                this.adscode = value;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 广告类型
        /// </summary>
        [Column("adstype")]
        public AdsType AdsType { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [Column("width")]
        public string AdsWidth { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        [Column("height")]
        public string AdsHeight { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column("indate")]
        public DateTime InDate { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [Column("tags")]
        public string Tags { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        [Column("introduce")]
        public string Introduce { get; set; }

        #endregion Model
    }

    [Table("bitcms_adsdetail")]
    public class AdsDetailInfo
    {
        /// <summary>
        /// 广告内容Id
        /// </summary>
        [Column("adsdetailid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdsDetailId { get; set; }
        private string adscode = "";
        /// <summary>
        /// 广告编码
        /// </summary>
        [Column("adscode")]
        public string AdsCode
        {
            get
            {
                if (this.adscode == null)
                {
                    this.adscode = "";
                }
                return this.adscode.ToLower();
            }
            set
            {
                this.adscode = value;
            }
        }
        /// <summary>
        /// 广告标题
        /// </summary>
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 广告链接地址
        /// </summary>
        [Column("link")]
        public string Link { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        [Column("path")]
        public string Path { get; set; }
        /// <summary>
        /// 描述，也存广告为代码的内容
        /// </summary>
        [Column("description")]
        public string Description { get; set; }
        /// <summary>
        /// PV值
        /// </summary>
        [Column("shownum")]
        public int ShowNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column("orderno")]
        public int OrderNo { get; set; }
        /// <summary>
        /// 广告类型
        /// </summary>
        [Column("adstype")]
        public AdsType AdsType { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        [Column("hitsnum")]
        public Int64 HitsNum { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [Column("width")]
        public string Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        [Column("height")]
        public string Height { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [Column("tag")]
        public string Tag { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Column("startdate")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Column("enddate")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 展示天数
        /// </summary>
        [Column("showdays")]
        public int ShowDays { get; set; }

    }
}
