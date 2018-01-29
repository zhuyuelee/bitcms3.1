using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_dictionary")]
    public class DictionaryInfo
    {
        #region Model
        /// <summary>
        /// KeyId
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("dictionaryid")]
        public int DictionaryId { get; set; }

        private string dictionarycode = "";
        /// <summary>
        /// 编码
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
        /// 名称
        /// </summary>
        [Column("name")]
        public string Name { get; set; }
        /// <summary>
        /// key1
        /// </summary>
        [Column("key")]
        public string Key { get; set; }
        /// <summary>
        /// key2
        /// </summary>
        [Column("key2")]
        public string Key2 { get; set; }
        /// <summary>
        /// key2type
        /// </summary>
        [Column("key2type")]
        public string Key2Type { get; set; }
        /// <summary>
        /// key3
        /// </summary>
        [Column("key3")]
        public string Key3 { get; set; }
        /// <summary>
        /// key3type
        /// </summary>
        [Column("key3type")]
        public string Key3Type { get; set; }
        /// <summary>
        /// key4
        /// </summary>
        [Column("key4")]
        public string Key4 { get; set; }
        /// <summary>
        /// key4type
        /// </summary>
        [Column("key4type")]
        public string Key4Type { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        [Column("explain")]
        public string Explain { get; set; }
        #endregion Model

    }
}
