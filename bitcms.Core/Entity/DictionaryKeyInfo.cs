using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_DictionaryKey")]
    public class DictionaryKeyInfo
    {
        #region Model
        /// <summary>
        /// KeyId
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("keyid")]
        public int KeyId { get; set; }

        /// <summary>
        /// KeyId
        /// </summary>
        [Column("dictionaryid")]
        public int DictionaryId { get; set; }

        private string dictionarycode = "";
        /// <summary>
        /// 字典编码
        /// </summary>
        [Column("dictionarycode")]
        public string DictionaryCode
        {
            get
            {
                if (this.dictionarycode == null)
                {
                    this.dictionarycode = "";
                }
                return this.dictionarycode.ToLower();
            }
            set
            {
                this.dictionarycode = value;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [Column("title")]
        public string Title { get; set; }

        private string value = "";
        /// <summary>
        /// 值
        /// </summary>
        [Column("value")]
        public string Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = "";
                }
                return this.value.ToLower();
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// 值2
        /// </summary>
        [Column("value2")]
        public string Value2 { get; set; }

        /// <summary>
        /// 值3
        /// </summary>
        [Column("value3")]
        public string Value3 { get; set; }

        /// <summary>
        /// 值4
        /// </summary>
        [Column("value4")]
        public string Value4 { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        [Column("explain")]
        public string Explain { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column("orderno")]
        public int OrderNo { get; set; }

        /// <summary>
        /// 系统设置
        /// </summary>
        [Column("systemsetting")]
        public int SystemSetting { get; set; }
        #endregion Model

    }
}
