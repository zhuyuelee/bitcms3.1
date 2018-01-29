using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitcms.Entity
{
    [Table("bitcms_favorites")]
    [Serializable]
    public class FavoritesInfo
    {
        /// <summary>
        /// 收藏夹id
        /// </summary>		
        [Column("favoritesid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FavoritesId { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>		
        [Column("userid")]
        public int UserId { get; set; }
        /// <summary>
        /// 收藏夹编码
        /// </summary>		
        [Column("favoritescode")]
        public string FavoritesCode { get; set; }
        /// <summary>
        /// 收藏目标Id
        /// </summary>		
        [Column("targetid")]
        public int TargetId { get; set; }
        /// <summary>
        /// 收藏标题
        /// </summary>		
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>		
        [Column("pic")]
        public string Pic { get; set; }
        /// <summary>
        /// 地址
        /// </summary>		
        [Column("link")]
        public string Link { get; set; }
        /// <summary>
        /// 描述
        /// </summary>		
        [Column("describe")]
        public string Describe { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }        
    }
}
