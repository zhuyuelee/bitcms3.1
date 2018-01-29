using System;
using System.Collections.Generic;

namespace bitcms.Entity
{
    /// <summary>
    /// 会员在线
    /// </summary>
    [Serializable]
    public class UserOnlineInfo : BasicUserOnlineInfo
    {

        #region 会员在线属性定义

        /// <summary>
        /// 管理员用户信息
        /// </summary>
        public UserInfo AdminUserInfo { get; set; }

        /// <summary>
        /// 授权会员
        /// </summary>
        public UserBindInfo UserBindInfo { get; set; }
        /// <summary>
        /// 授权登录
        /// </summary>
        public bool IsOAuth { get; set; }



        /// <summary>
        /// 最后刷新时间
        /// </summary>
        public System.DateTime RefreshDate { get; set; }

        /// <summary>
        /// 保存登录信息
        /// </summary>
        public bool RememberUser { get; set; }

        /// <summary>
        /// 管理员是否在线
        /// </summary>
        public bool AdminOnline
        {
            //三十分钟过期
            get { return this.AdminUserInfo != null && this.RefreshDate.AddMinutes(30) > DateTime.Now; }
        }

        private List<VerifyCode> verifyCode = new List<VerifyCode>();
        /// <summary>
        /// 验证码
        /// </summary>
        public List<VerifyCode> VerifyCode { get { return this.verifyCode; } }

        /// <summary>
        /// 安全key
        /// </summary>
        public string SafeKey { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int VisitsNum { get; set; }

        /// <summary>
        /// 不安全访问开始统计时间
        /// </summary>
        public System.DateTime StatisticsDate { get; set; }

        /// <summary>
        /// 最后评论时间
        /// </summary>
        public System.DateTime? LastReviewDate { get; set; }
        #endregion

    }

    [Serializable]
    public class BasicUserOnlineInfo
    {
        /// <summary>
        /// 会员在线Id
        /// </summary>
        public String UserOnlineId { get; set; }

        /// <summary>
        /// 前台会员信息
        /// </summary>
        public BasicUserInfo UserInfo { get; set; }

        private int userid = -1;
        /// <summary>
        /// 会员id
        /// </summary>
        public int UserId
        {
            get { return userid; }
            set { userid = value; }
        }

        /// <summary>
        /// 会员
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 不安全访问次数
        /// </summary>
        public int UnsafeVisitsNum { get; set; }

        /// <summary>
        /// 登录验证码
        /// </summary>
        public bool LoginVerifykey
        {
            get
            {
                var config = Config.SiteConfig.load();
                if (config.EnabledLoginVerifykey == 1 || (config.EnabledLoginVerifykey == 2 && this.UnsafeVisitsNum > 3))
                {
                    return true;
                }
                return false;
            }
            
        }

        /// <summary>
        /// 上级页面
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// 最后位置
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 在线IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 开始访问时间
        /// </summary>
        public System.DateTime StartDate { get; set; }

    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 验证账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 验证过期时间
        /// </summary>
        public DateTime? Deadline { get; set; }
    }
}
