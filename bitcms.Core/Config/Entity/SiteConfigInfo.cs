using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using bitcms.Common;

namespace bitcms.Entity
{
    /// <summary>
    /// 网站配置
    /// </summary>
    [Serializable]
    [XmlRoot("Root")]
    public class SiteConfigInfo : NormalConfigInfo
    {
        #region 安全配置
        /// <summary>
        /// 网站编码
        /// </summary>
        public string SiteCode { get; set; }
       
        /// <summary>
        /// Cookie域
        /// </summary>
        public string CookieDomain { get; set; }

        /// <summary>
        /// 启用Wap站
        /// </summary>
        public bool EnabledWap { get; set; }

        private string _namespace = "bitcms.Web.Controllers";
        /// <summary>
        /// 前台控制器命名空间
        /// </summary>
        public string NameSpace
        {
            get { return this._namespace; }
            set { this._namespace = value; }
        }

        private string home = "main";
        /// <summary>
        /// 首页控制器
        /// </summary>
        public string Home
        {
            get { return this.home; }
            set { this.home = value; }
        }
        /// <summary>
        /// 匿名提交
        /// </summary>
        public string VerifyPost { get; set; }
        /// <summary>
        /// 会员允许重名
        /// </summary>
        public bool DuplicationUserName { get; set; }
        /// <summary>
        /// 登录验证码 0不启用 1启用 2智能启用
        /// </summary>
        public int EnabledLoginVerifykey { get; set; }
        /// <summary>
        /// 验证码背景
        /// </summary>
        public string VerifykeyBackGround { get; set; }
        /// <summary>
        /// IP禁止访问列表
        /// </summary>
        public string IpDenyAccess { get; set; }

        /// <summary>
        /// 管理员后台IP访问列表
        /// </summary>
        public string AdminIpAccess { get; set; }

        /// <summary>
        /// 错误日志状态
        /// </summary>
        public int DebugLevel { get; set; }

        #endregion

        #region 内容设置
        /// <summary>
        /// 人工审核会员文章
        /// </summary>
        public bool VerifyUserDetail { get; set; }

        /// <summary>
        ///审核评论
        /// </summary>
        public bool VerifyReview { get; set; }

        private int reviewintervaltime = 10;
        /// <summary>
        ///评论间隔时间限制 0为不限制
        /// </summary>
        public int ReviewIntervalTime
        {
            get { return this.reviewintervaltime; }
            set { this.reviewintervaltime = value; }
        }

        /// <summary>
        /// 重复点赞
        /// </summary>
        public bool RepeatAgree { get; set; }

        private int agreeintervaltime = 10;
        /// <summary>
        ///点赞间隔时间限制 0为不限制
        /// </summary>
        public int AgreeIntervalTime
        {
            get { return this.agreeintervaltime; }
            set { this.agreeintervaltime = value; }
        }
        #endregion
    }

    /// <summary>
    /// 网站基本配置
    /// </summary>
    public class NormalConfigInfo
    {
        #region 网站基本配置
        private string siteDomain;
        /// <summary>
        /// 站点域名
        /// </summary>
        [Required]
        public string SiteDomain
        {
            get { return this.siteDomain; }
            set
            {
                var domain = value.ToLower();
                if (!(domain.StartsWith("http://") || domain.StartsWith("https://")))
                {
                    domain = "http://" + domain;
                }
                if (!domain.EndsWith("/"))
                {
                    domain += "/";
                }
                this.siteDomain = domain;
            }
        }

        private string sitepath = "/";
        /// <summary>
        /// 网站目录
        /// </summary>
        [Required]
        public string SitePath
        {
            get { return this.sitepath; }
            set { this.sitepath = value; }
        }
        /// <summary>
        /// 站点名
        /// </summary>
        [Required]
        public string SiteName { get; set; }

        /// <summary>
        /// 时区
        /// </summary>
        public int TimeZone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 网站备案信息
        /// </summary>
        public string Icp { get; set; }

        /// <summary>
        /// 统计代码
        /// </summary>
        public string StatisticsCode { get; set; }


        /// <summary>
        /// 关闭网站
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// 关闭提示信息
        /// </summary>
        public string ClosedMessage { get; set; }


        /// <summary>
        /// 标题附加字
        /// </summary>
        public string SeoTitle { get; set; }

        /// <summary>
        /// Meta Keywords
        /// </summary>
        public string SeoKeywords { get; set; }

        /// <summary>
        /// Meta Description
        /// </summary>
        public string SeoDescription { get; set; }
        #endregion

        #region 验证码
       
        /// <summary>
        /// 启用注册验证码
        /// </summary>
        public bool EnabledRegisterVerifykey { get; set; }
        /// <summary>
        /// 启用资讯验证码
        /// </summary>
        public bool EnabledDetailVerifykey { get; set; }
        /// <summary>
        /// 启用评论验证码
        /// </summary>
        public bool EnabledReviewVerifykey { get; set; }

        #endregion

        #region 配置
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return Config.SiteConfig.Version; }
        }
        /// <summary>
        /// 版本名
        /// </summary>
        public string VersionName
        {
            get { return Config.SiteConfig.VersionName; }
        }
        /// <summary>
        /// 公司
        /// </summary>
        public string AssemblyCompany
        {
            get { return Config.SiteConfig.AssemblyCompany; }
        }
        /// <summary>
        /// 产品名
        /// </summary>
        public string AssemblyProduct
        {
            get { return Config.SiteConfig.AssemblyProduct; }
        }
        /// <summary>
        /// Copyright
        /// </summary>
        public string AssemblyCopyright
        {
            get { return Config.SiteConfig.AssemblyCopyright; }
        }
        #endregion

       
    }
}
