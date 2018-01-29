using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_city")]
    public class CityInfo
    {
        #region Model
        /// <summary>
        /// 城市编码
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("cityid")]
        public int CityId{ get; set; }
       
        /// <summary>
        /// 上级城市编码
        /// </summary>
        [Column("fathercityid")]
        public int FatherCityId { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        [Column("cityname")]
        public string CityName { get; set; }

        [Column("diliquhua")]
        public string DiliQuhua { get; set; }
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

        /// <summary>
        /// 邮编
        /// </summary>
        [Column("postcode")]
        public string PostCode { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        [Column("deep")]
        public int Deep { get; set; }
        #endregion Model
    }
}
