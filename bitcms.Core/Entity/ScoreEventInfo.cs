using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_scoreevent")]
    [Serializable]
    public class ScoreEventInfo
    {
        /// <summary>
        /// 事件id
        /// </summary>		
        [Column("eventid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        private string eventcode = "";
        /// <summary>
        /// 事件编码
        /// </summary>		
        [Column("eventcode")]
        public string EventCode
        {
            get
            {
                if (this.eventcode == null)
                {
                    this.eventcode = "";
                }
                return this.eventcode.ToLower();
            }
            set
            {
                this.eventcode = value;
            }
        }
        /// <summary>
        /// 事件名
        /// </summary>		
        [Column("eventname")]
        public string EventName { get; set; }
        /// <summary>
        /// 启用
        /// </summary>		
        [Column("enabled")]
        public int Enabled { get; set; }
        /// <summary>
        /// 积分值
        /// </summary>		
        [Column("score")]
        public int Score { get; set; }
        /// <summary>
        /// 方向 0加 1减
        /// </summary>		
        [Column("direction")]
        public int Direction { get; set; }
        /// <summary>
        /// 事件类型 0系统事件 1人工操作
        /// </summary>		
        [Column("eventtype")]
        public int EventType { get; set; }
        /// <summary>
        /// 说明
        /// </summary>		
        [Column("explan")]
        public string Explan { get; set; }
        /// <summary>
        /// 次数
        /// </summary>		
        [Column("times")]
        public int Times { get; set; }
        /// <summary>
        /// 次数
        /// </summary>		
        [Column("period")]
        public int Period { get; set; }       
    }
}
