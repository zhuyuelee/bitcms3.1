using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bitcms.Entity
{
    [Table("bitcms_scorelog")]
    [Serializable]
    public class ScoreLogInfo : BasicScoreLogInfo
    {
         
    }

    public class ScoreLogsInfo : BasicScoreLogInfo
    {
        public string UserName { get; set; }
    }

    public class BasicScoreLogInfo
    {
        /// <summary>
        /// 记录id
        /// </summary>		
        [Column("logid")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        /// <summary>
        /// 事件id
        /// </summary>		
        [Column("eventid")]
        public int EventId { get; set; }
        /// <summary>
        /// 会员Id
        /// </summary>		
        [Column("userid")]
        public int UserId { get; set; }
        /// <summary>
        /// 积分值 
        /// </summary>		
        [Column("score")]
        public int Score { get; set; }
        /// <summary>
        /// 收入或支出 0收入 1支出
        /// </summary>		
        [Column("incomeorexpenses")]
        public int IncomeOrExpenses { get; set; }
        /// <summary>
        /// 理由
        /// </summary>		
        [Column("reason")]
        public string Reason { get; set; }
        /// <summary>
        /// 时间
        /// </summary>		
        [Column("indate")]
        public DateTime InDate { get; set; }      
    }
}
