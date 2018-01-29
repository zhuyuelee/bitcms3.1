using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_item")]
    [Serializable]
    public class ItemInfo
    {
        /// <summary>
        /// 栏目id
        /// </summary>		
        [Column("itemid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        /// <summary>
        /// 父栏目id
        /// </summary>		
        [Column("fatherid")]
        public int FatherId { get; set; }
        /// <summary>
        /// 频道编码
        /// </summary>		
        [Column("channelcode")]
        public string ChannelCode { get; set; }
        /// <summary>
        /// 栏目编码
        /// </summary>		
        [Column("itemcode")]
        public string ItemCode { get; set; }
        /// <summary>
        /// 栏目名
        /// </summary>		
        [Column("itemname")]
        public string ItemName { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>		
        [Column("show")]
        public int Show { get; set; }
        /// <summary>
        /// 排序
        /// </summary>		
        [Column("orderno")]
        public int OrderNo { get; set; }

        private string explain = "";
        /// <summary>
        /// 分类说明
        /// </summary>		
        [Column("explain")]
        public string Explain
        {
            get
            {
                if (this.explain == null)
                {
                    this.explain = "";
                }
                return this.explain;
            }
            set { explain = value; }
        }
        /// <summary>
        /// 阅读权限值
        /// </summary>		
        [Column("readpower")]
        public int ReadPower { get; set; }
        /// <summary>
        /// 图标
        /// </summary>		
        [Column("icon")]
        public string Icon { get; set; }
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

        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }

    }
}
