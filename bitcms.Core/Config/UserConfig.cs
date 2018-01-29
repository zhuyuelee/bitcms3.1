using System;
using System.Collections.Generic;
using bitcms.Common;
using bitcms.Entity;

namespace bitcms.Config
{
    public class UserConfig
    {
        private static Dictionary<string, UserOnlineInfo> dicUserOnline = null;
        /// <summary>
        /// 字典key
        /// </summary>
        private static string getKey
        {
            get
            {
                var key = Cookie.getCookie("bitcms", "olid");
                if (string.IsNullOrEmpty(key))
                {
                    key = string.Empty;
                }
                return key;
            }
            set
            {
                Cookie.writeCookie("bitcms", "olid", value);
            }
        }

        /// <summary>
        /// 字典过期时间
        /// </summary>
        private static void setKeyExpires(int days)
        {
            var key = getKey;
            Cookie.writeCookie("bitcms", "olid", key, days * 60 * 24);
        }
        /// <summary>
        /// 获取设置在线会员
        /// </summary>
        private static UserOnlineInfo getOnlineUser
        {
            get
            {
                var key = getKey;

                if (!string.IsNullOrEmpty(key) && dicUserOnline.ContainsKey(key))
                {
                    var useronline = dicUserOnline[key];

                    return useronline;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                var key = getKey;
                if (string.IsNullOrEmpty(key))
                {
                    key = getKey = value.UserOnlineId;
                }
                if (dicUserOnline.ContainsKey(key))
                {
                    dicUserOnline[key] = value;
                }
                else
                {
                    dicUserOnline.Add(key, value);
                }
            }
        }

        static UserConfig()
        {
            if (dicUserOnline == null)
            {
                dicUserOnline = new Dictionary<string, UserOnlineInfo>();
            }

        }
        /// <summary>
        /// 获取在线人数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static int getUserOnlneCount()
        {
            return dicUserOnline.Values.Count;
        }

        /// <summary>
        /// 获取在线人数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<Entity.BasicUserInfo> getUserOnlne()
        {
            var list = new List<Entity.BasicUserInfo>();

            foreach (var user in dicUserOnline)
            {
                list.Add(user.Value.UserInfo);
            }

            return list;
        }
        /// <summary>
        /// 管理员退出
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static void adminLogout()
        {
            var onlineInfo = getUserOnlineInfo();
            if (onlineInfo != null)
            {
                getOnlineUser.AdminUserInfo = null;
            }
        }

        /// <summary>
        /// 根据清除登陆信息
        /// </summary>
        public static void clearUserOline()
        {
            var onlineInfo = getUserOnlineInfo();
            if (onlineInfo != null)
            {
                getOnlineUser.UserInfo = getVisitor();
                onlineInfo.UserId = -1;
                getOnlineUser = onlineInfo;
            }
        }

        /// <summary>
        /// 更新浏览记录
        /// </summary>
        private static void browseHistory(UserOnlineInfo onlineInfo, bool safe = false)
        {
            var ip = Fetch.getClientIP();

            var date = SiteConfig.getLocalTime();
            var span = date - onlineInfo.StatisticsDate;
            if (!safe)
            {
                if (span.TotalSeconds < 1.0)
                {
                    onlineInfo.StatisticsDate = date;
                    onlineInfo.UnsafeVisitsNum++;
                }
                else if (span.TotalSeconds > 10 && onlineInfo.UnsafeVisitsNum > 5)//会员登录错误次数在5以下使用
                {
                    onlineInfo.UnsafeVisitsNum--;
                }
            }
            onlineInfo.RefreshDate = date;
            onlineInfo.UrlReferrer = onlineInfo.Url;
            onlineInfo.Url = Fetch.getRawUrl();


        }

        /// <summary>
        /// 根据会员在线ID获取在线会员信息
        /// </summary>
        /// <param name="userOnlineId"></param>
        /// <returns></returns>
        public static UserOnlineInfo getUserOnlineInfo(bool safe = false)
        {
            var info = getOnlineUser;
            if (info == null)
            {
                info = new UserOnlineInfo()
                {
                    UserOnlineId = Guid.NewGuid().ToString("N"),
                    IP = Fetch.getClientIP(),
                    StartDate = SiteConfig.getLocalTime(),
                    Url = Fetch.getRawUrl(),
                    UnsafeVisitsNum = -1,
                    StatisticsDate = SiteConfig.getLocalTime().AddMinutes(10),
                };

                info.SafeKey = Utils.MD5(info.IP);
                info.UserInfo = getVisitor();

                getOnlineUser = info;
            }

            browseHistory(info, safe);


            return info;
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="info"></param>
        /// <param name="account">账号</param>
        /// <param name="code">验证码</param>
        /// <param name="minute">分钟</param>
        public static Entity.VerifyCode setVerifyCode(string account, string code, int minute = 0)
        {
            var info = getOnlineUser;
            return setVerifyCode(info, account, code, minute);
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="info"></param>
        /// <param name="account">账号</param>
        /// <param name="code">验证码</param>
        /// <param name="minute">分钟</param>
        public static Entity.VerifyCode setVerifyCode(UserOnlineInfo info, string account, string code, int minute = 0)
        {
            var codeList = info.VerifyCode;
            Entity.VerifyCode codeInfo = codeList.Find(g => g.Account.Equals(account));
            if (codeInfo == null)
            {
                codeInfo = new Entity.VerifyCode();
                codeInfo.Account = account.ToLower();
                info.VerifyCode.Add(codeInfo);
            }
            codeInfo.Code = code.ToLower();
            if (minute > 0)
            {
                codeInfo.Deadline = Config.SiteConfig.getLocalTime().AddMinutes(minute);
            }
            else
            {
                codeInfo.Deadline = null;
            }

            return codeInfo;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="account">账号</param>
        public static Entity.VerifyCode getVerifyCode(string account, bool clear = true)
        {
            var info = getOnlineUser;
            var codeInfo = info.VerifyCode.Find(g => g.Account.Equals(account));
            if (codeInfo != null)
            {
                if (codeInfo.Deadline != null && codeInfo.Deadline < Config.SiteConfig.getLocalTime())
                {
                    codeInfo = null;
                }
                if (clear)
                {
                    info.VerifyCode.Remove(codeInfo);
                }
            }
            return codeInfo;
        }

        /// <summary>
        /// 设置游客在线信息
        /// </summary>
        /// <returns></returns>
        private static BasicUserInfo getVisitor()
        {
            return new Entity.BasicUserInfo()
            {
                Avatar = string.Format("{0}images/noavatar_small.gif", Config.SiteConfig.getSitePath()),
                UserId = -1,
                UserName = "游客",
                RoleId = -1,
            };
        }

        #region 记录在线会员信息
        /// <summary>
        /// 设置会员在线
        /// </summary>
        /// <param name="userInfo"></param>
        public static void setUserOnline(BasicUserInfo userInfo, int days = 0)
        {
            var onlineInfo = getUserOnlineInfo();

            if (days > 0)
            {
                setKeyExpires(days);
            }

            onlineInfo.UserInfo = userInfo;
            onlineInfo.UserName = userInfo.UserName;
            onlineInfo.UserId = userInfo.UserId;
            getOnlineUser = onlineInfo;

        }
        /// <summary>
        /// 设置管理员在线
        /// </summary>
        /// <param name="userInfo"></param>
        public static void setAdminOnline(UserInfo userInfo)
        {
            var onlineInfo = getUserOnlineInfo();
            onlineInfo.AdminUserInfo = userInfo;
        }
        /// <summary>
        /// 设置授权会员在线
        /// </summary>
        /// <param name="userInfo"></param>
        public static void setOAuthOnline(UserBindInfo userBindInfo)
        {
            var onlineInfo = getUserOnlineInfo();
            onlineInfo.UserBindInfo = userBindInfo;
            if (onlineInfo.UserId <= 0 && userBindInfo.UserId > 0)//设置会员在线
            {
                onlineInfo.UserInfo.UserId = userBindInfo.UserId;
                onlineInfo.UserId = userBindInfo.UserId;
            }
            onlineInfo.UserInfo.UserName = userBindInfo.NickName;
            onlineInfo.UserName = userBindInfo.NickName;
            onlineInfo.IsOAuth = true;
            getOnlineUser = onlineInfo;
        }
        #endregion

    }
}
