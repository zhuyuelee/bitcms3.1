using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using bitcms.Common;
using bitcms.UI;

namespace bitcms.Web.Controllers
{
    /// <summary>
    /// 提交获取数据
    /// </summary>
    public class CMSActionController : BaseActionController
    {
        #region 会员
        /// <summary>
        /// 积分操作
        /// </summary>
        /// <returns>1成功 0失败</returns>
        [HttpPost]
        public ActionResult setUserScore(string eventcode)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var result = manage.insertScoreLog(this.userOnlineInfo.UserId, eventcode);
                return this.getResult(Entity.Error.请求成功, "请求成功！", result ? 1 : 0);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult signout()
        {
            Config.UserConfig.clearUserOline();
            return this.getResult(Entity.Error.请求成功, "请求成功！");
        }

        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult getLoginState()
        {
            return this.getResult(this.userOnlineInfo.UserInfo);
        }

        #region 注册
        /// <summary>
        /// 会员第三方账号注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult registerAccount(string account, string password, string accounttype, Entity.UserInfo userinfo, string verifykey)
        {
            var code = Config.UserConfig.getVerifyCode(account);
            if (code != null && code.Code.Equals(verifykey.ToLower()))
            {
                if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(accounttype))
                {
                    accounttype = accounttype.ToLower();
                    if (accounttype == "mobile")
                    {
                        if (!Utils.isMobile(account))
                        {
                            return this.getResult(Entity.Error.错误, "手机格式正确");
                        }
                    }
                    else if (accounttype == "email")
                    {
                        if (!Utils.isEmail(account))
                        {
                            return this.getResult(Entity.Error.错误, "email格式不正确");
                        }
                    }
                    using (var manage = new Data.CMSManage())
                    {
                        if (manage.checkUserBindCode(accounttype, account) > 0)
                        {
                            return this.getResult(Entity.Error.错误, "账号已存在");
                        }
                        if (userinfo == null)
                        {
                            userinfo = new Entity.UserInfo();
                        }
                        if (string.IsNullOrEmpty(userinfo.UserName))
                        {
                            if (accounttype == "mobile")
                            {
                                userinfo.UserName = Utils.fuzzyString(account, 3, 4);
                            }
                            else if (accounttype == "email")
                            {
                                var name = Utils.subString(account, account.IndexOf('@'));
                                userinfo.UserName = Utils.fuzzyString(account, 1, name.Length - 1);
                            }
                            else
                            {
                                userinfo.UserName = Utils.fuzzyString(account, 1, account.Length - 1);
                            }
                        }
                        else if (manage.checkUserName(userinfo.UserName, -1) > 0)
                        {
                            return this.getResult(Entity.Error.错误, "昵称已存在");
                        }

                        userinfo.InDate = DateTime.Now;
                        userinfo.IP = Fetch.getClientIP();

                        //默认会员
                        if (userinfo.RoleId <= 0)
                        {
                            var roleInfo = manage.getRoleByScore("score", 0);
                            if (roleInfo != null)
                            {
                                userinfo.RoleId = roleInfo.RoleId;
                                userinfo.VerifyMember = 1;
                            }
                        }

                        //提交注册
                        manage.updateUser(userinfo);

                        if (userinfo.UserId > 0)
                        {

                            //设置密码
                            manage.updatePassword(userinfo.UserId, Entity.passwordType.user, password);
                            //绑定会员
                            manage.updateUserBind(accounttype, userinfo.UserId, account, userinfo.UserName, 1);

                            //更改积分
                            manage.insertScoreLog(userinfo.UserId, "register");

                            //设置会员在线
                            Config.UserConfig.setUserOnline(userinfo);
                        }
                        return this.getResult(Entity.Error.请求成功, "注册成功", new { userid = userinfo.UserId });
                    }
                }
                else
                {
                    return this.getResult(Entity.Error.错误, "数据不完整");
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "验证码错误");
            }
        }
        /// <summary>
        /// 会员账号注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult register(Entity.UserInfo userinfo, string password, string key = null, string verifykey = null)
        {
            if (this.config.EnabledRegisterVerifykey)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "verifycode";
                }
                var code = Config.UserConfig.getVerifyCode(key);
                if (string.IsNullOrEmpty(verifykey) || code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误！");
                }
            }


            if (userinfo != null && !string.IsNullOrEmpty(userinfo.UserName) && !string.IsNullOrEmpty(password))
            {
                using (var manage = new Data.CMSManage())
                {
                    if (manage.checkUserName(userinfo.UserName, -1) > 0)
                    {
                        return this.getResult(Entity.Error.错误, "账号已存在");
                    }

                    userinfo.InDate = DateTime.Now;
                    userinfo.IP = Fetch.getClientIP();
                    userinfo.VerifyMember = 1;

                    //默认会员
                    if (userinfo.RoleId <= 0)
                    {
                        var roleInfo = manage.getRoleByScore("score", 0);
                        if (roleInfo != null)
                        {
                            userinfo.RoleId = roleInfo.RoleId;
                        }
                    }

                    //提交注册
                    manage.updateUser(userinfo);

                    if (userinfo.UserId > 0)
                    {
                        //设置密码
                        manage.updatePassword(userinfo.UserId, Entity.passwordType.user, password);

                        //更改积分
                        manage.insertScoreLog(userinfo.UserId, "register");

                        //设置会员在线
                        Config.UserConfig.setUserOnline(userinfo);
                    }
                    return this.getResult(Entity.Error.请求成功, "注册成功", new { userid = userinfo.UserId });
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "数据不完整");
            }

        }
        #endregion

        #region 登录
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult login(string username, string password, string key, string verifykey, int expires = 0)
        {
            if (this.userOnlineInfo.LoginVerifykey)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "verifycode";
                }
                var code = Config.UserConfig.getVerifyCode(key);
                if (string.IsNullOrEmpty(verifykey) || code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误！", new { loginVerifykey = this.userOnlineInfo.LoginVerifykey });
                }
            }

            return checkUserPasword(username, password, expires);
        }

        /// <summary>
        /// 第三方账号登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult loginAccount(string account, string password, string accounttype, string key, string verifykey, int expires = 0)
        {
            if (this.userOnlineInfo.LoginVerifykey)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "verifycode";
                }
                var code = Config.UserConfig.getVerifyCode(key);
                if (string.IsNullOrEmpty(verifykey) || code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误！", new { loginVerifykey = this.userOnlineInfo.LoginVerifykey });
                }
            }
            using (var manage = new bitcms.Data.CMSManage())
            {
                if (string.IsNullOrEmpty(accounttype))
                {
                    if (Common.Utils.isMobile(account))
                    {
                        accounttype = "mobile";
                    }
                    else if (Common.Utils.isEmail(account))
                    {
                        accounttype = "email";
                    }
                }
                if (!string.IsNullOrEmpty(accounttype))
                {
                    var moduleInfo = manage.getModuleInfo(accounttype);
                    if (moduleInfo != null && moduleInfo.Enabled == 1)
                    {
                        var userBindInfo = manage.getUserBindInfo(accounttype, account);
                        if (userBindInfo != null)
                        {
                            if (manage.checkUserPassword(userBindInfo.UserId, password, Entity.passwordType.user))
                            {
                                this.userOnlineInfo.UnsafeVisitsNum = 0;
                                //设置会员在线
                                var userinfo = manage.getUserInfo(userBindInfo.UserId);
                                manage.updateLastLandDate(userinfo);

                                if (this.userOnlineInfo.IsOAuth)//更新授权登陆会员
                                {
                                    this.userOnlineInfo.UserBindInfo.UserId = userinfo.UserId;
                                    manage.updateUserBind(this.userOnlineInfo.UserBindInfo);
                                }

                                Config.UserConfig.setUserOnline(userinfo, expires);

                                //更改积分
                                manage.insertScoreLog(userinfo.UserId, "land");

                                return this.getResult(Entity.Error.请求成功, "登录成功！");
                            }
                            else
                            {
                                this.userOnlineInfo.UnsafeVisitsNum++;
                                return this.getResult(Entity.Error.错误, "密码错误！", new { loginVerifykey = this.userOnlineInfo.LoginVerifykey });
                            }
                        }
                        else
                        {
                            this.userOnlineInfo.UnsafeVisitsNum++;
                            return this.getResult(Entity.Error.错误, "账号不存在！", new { loginVerifykey = this.userOnlineInfo.LoginVerifykey });
                        }
                    }
                    else
                    {
                        return this.getResult(Entity.Error.错误, "配置错误，请与网站管理员联系！", new { loginVerifykey = this.userOnlineInfo.LoginVerifykey });
                    }
                }
                else
                {
                    //账号登录
                    return checkUserPasword(account, password, expires);
                }
            }
        }

        /// <summary>
        /// 检查登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private ActionResult checkUserPasword(string username, string password, int expires = 0)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var userInfo = manage.checkLogin(username, password, Entity.passwordType.user);

                if (userInfo == null)
                {
                    this.userOnlineInfo.UnsafeVisitsNum++;
                    return this.getResult(Entity.Error.错误, "验证失败！", new { loginVerifykey = this.userOnlineInfo.LoginVerifykey });
                }
                else
                {
                    this.userOnlineInfo.UnsafeVisitsNum = 0;
                    if (this.userOnlineInfo.IsOAuth)//更新授权登陆会员
                    {
                        this.userOnlineInfo.UserBindInfo.UserId = userInfo.UserId;
                        manage.updateUserBind(this.userOnlineInfo.UserBindInfo);
                    }

                    //设置会员在线
                    Config.UserConfig.setUserOnline(userInfo, expires);
                    //更改积分
                    manage.insertScoreLog(userInfo.UserId, "land");

                    return this.getResult(Entity.Error.请求成功, "登录成功！");
                }
            }
        }

        #endregion
        /// <summary>
        /// 检查账号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult checkAccount(string account, string accounttype, string verifykey = null)
        {
            //验证码
            if (!string.IsNullOrEmpty(verifykey))
            {
                var code = Config.UserConfig.getVerifyCode(account);
                if (code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误或已经过期");
                }
            }
            using (var manage = new bitcms.Data.CMSManage())
            {
                var userid = manage.checkUserBindCode(accounttype, account);
                return this.getResult(manage.Error, manage.Message, new { UserId = userid });
            }
        }

        /// <summary>
        /// 检查用户名
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult checkUserName(string username)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var count = manage.checkUserName(username, this.userOnlineInfo.UserId);
                return this.getResult(manage.Error, manage.Message, new { Count = count });
            }
        }
        /// <summary>
        /// 更新会员账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newmobile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updateAccount(string oldaccount, string account, string accounttype, string verifykey)
        {
            var code = Config.UserConfig.getVerifyCode(account);
            if (code != null && code.Code.Equals(verifykey.ToLower()))
            {
                using (var manage = new bitcms.Data.CMSManage())
                {
                    var userBindInfo = manage.getUserBindInfo(accounttype, account);
                    if (userBindInfo != null)
                    {
                        return this.getResult(Entity.Error.错误, "该账号已经绑定");
                    }
                    else
                    {
                        manage.updateUserCode(this.userOnlineInfo.UserId, accounttype, oldaccount, account);
                        return this.getResult(manage.Error, manage.Message);
                    }
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "验证码错误或已经过期");
            }
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="newmobile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updateUserPassword(string oldpassword, string password)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                manage.updatePassword(this.userOnlineInfo.UserId, Entity.passwordType.user, oldpassword, password);
                return this.getResult(manage.Error, manage.Message);
            }
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getpassword(string account, string accounttype, string password, string verifycode)
        {
            string message = string.Empty;
            Entity.Error error = Entity.Error.请求成功;

            var mobilecode = Config.UserConfig.getVerifyCode(account);
            if (mobilecode != null && mobilecode.Code == verifycode && mobilecode.Deadline >= Config.SiteConfig.getLocalTime())
            {
                if (mobilecode.Account == account)
                {
                    using (var manage = new bitcms.Data.CMSManage())
                    {
                        var userBindInfo = manage.getUserBindInfo(accounttype, account);
                        if (userBindInfo != null && userBindInfo.UserId > 0)
                        {
                            manage.updatePassword(userBindInfo.UserId, Entity.passwordType.user, password);
                        }
                    }
                }
                else
                {
                    error = Entity.Error.错误;
                    message = "验证账号和原账号不一致！";
                }
            }
            else
            {
                error = Entity.Error.错误;
                message = "验证码错误或已经过期！";
            }
            return this.getResult(error, message);
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updateAvatar(string avatar)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                manage.updateUserAvatar(this.userOnlineInfo.UserId, avatar);
                this.userOnlineInfo.UserInfo.Avatar = avatar;

                //更改积分
                manage.insertScoreLog(this.userOnlineInfo.UserId, "updateavatar");
                return this.getResult(manage.Error, manage.Message);

            }
        }

        /// <summary>
        /// 更新会员资料
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updateUser(Entity.UserInfo userinfo)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                userinfo.UserId = this.userOnlineInfo.UserId;
                if (string.IsNullOrEmpty(userinfo.UserName))
                {
                    userinfo.UserName = this.userOnlineInfo.UserName;
                }
                manage.updateBasicUser(userinfo);
                Config.UserConfig.setUserOnline(userinfo);

                return this.getResult(manage.Error, manage.Message);
            }
        }
        #endregion

        #region 内容
        /// <summary>
        /// 获取频道详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailChannel(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                using (var manage = new Data.CMSManage())
                {
                    var info = manage.getDetailChannelInfo(code);
                    return this.getResult(manage.Error, manage.Message, info);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "参数错误");
            }
        }

        /// <summary>
        /// 获取栏目详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getItem(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                using (var manage = new Data.CMSManage())
                {
                    var info = manage.getItemInfo(code);
                    return this.getResult(manage.Error, manage.Message, info);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "参数错误");
            }
        }
        /// <summary>
        /// 获取栏目详情
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getItem(int itemid)
        {
            if (itemid > 0)
            {
                using (var manage = new Data.CMSManage())
                {
                    var info = manage.getItemInfo(itemid);
                    return this.getResult(manage.Error, manage.Message, info);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "参数错误");
            }
        }
        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="code">频道编码</param>
        /// <param name="itemid"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getItemList(string code, int itemid, int show = -1)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getItemList(code, itemid, show);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }

        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetail(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                using (var manage = new Data.CMSManage())
                {
                    var info = manage.getDetailInfo(code);
                    return this.getResult(manage.Error, manage.Message, info);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "参数错误");
            }
        }
        /// <summary>
        /// 获取栏目详情
        /// </summary>
        /// <param name="detailid"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetail(int detailid)
        {
            if (detailid > 0)
            {
                using (var manage = new Data.CMSManage())
                {
                    var info = manage.getDetailInfo(detailid);
                    return this.getResult(manage.Error, manage.Message, info);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "参数错误");
            }
        }

        /// <summary>
        /// 获取内容列表
        /// </summary>
        /// <param name="top">前几条</param>
        /// <param name="code">频道编码</param>
        /// <param name="itemid">内容编码</param>
        /// <param name="show">推荐，不选请传-1</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailList(int top, string code, int itemid, int show = -1, string key = null)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getDetailList(top, code, itemid, show, key);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }

        /// <summary>
        /// 获取内容分页列表
        /// </summary>
        /// <param name="page">分页数据</param>
        /// <param name="code">频道编码</param>
        /// <param name="itemid">内容编码</param>
        /// <param name="userid">会员id</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailPageList(Entity.PageInfo page, string code, int itemid, int userid = -1)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getDetailList(page, code, itemid, userid);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }
        /// <summary>
        /// 获取内容详情
        /// </summary>
        /// <param name="detailid">内容编码</param>
        /// <param name="index">第几页,第一页为0</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailContent(int detailid, int index = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getDetailContentInfo(detailid, index);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }
        /// <summary>
        /// 获取内容详情列表
        /// </summary>
        /// <param name="detailid">内容编码</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailContentList(int detailid)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getDetailContentList(detailid);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }
        /// <summary>
        /// 获取内容详情列表
        /// </summary>
        /// <param name="code">频道编码</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailContentList(string code, string key)
        {
            using (var manage = new Data.CMSManage())
            {
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.ToLower();
                }
                if (!string.IsNullOrEmpty(key))
                {
                    key = key.ToLower();
                }
                var info = manage.getDetailContentList(code, key);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }
        /// <summary>
        /// 获取内容附件
        /// </summary>
        /// <param name="top">取前几条</param>
        /// <param name="detailid">detailid</param>
        /// <param name="type">附件类型 0图片（默认）、1音频、 2视频</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getDetailGalleryList(int top, int detailid, Entity.GalleryType type = Entity.GalleryType.picture, int iscover = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getDetailGalleryList(top, detailid, type, iscover == 1);
                return this.getResult(manage.Error, manage.Message, info);
            }
        }
        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult updateDetail(Entity.DetailInfo info, string pics, int cid, string content, string key, string verifykey)
        {
            if (this.config.EnabledDetailVerifykey)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "verifycode";
                }
                var code = Config.UserConfig.getVerifyCode(key);
                if (string.IsNullOrEmpty(verifykey) || code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误！");
                }
            }
            if (info == null || string.IsNullOrEmpty(info.ChannelCode))
            {
                return this.getResult(Entity.Error.错误, "频道错误！");
            }
            if (string.IsNullOrEmpty(pics) && string.IsNullOrEmpty(content))
            {
                return this.getResult(Entity.Error.错误, "内容为空！");
            }
            using (var manage = new bitcms.Data.CMSManage())
            {
                if (this.config.VerifyUserDetail)
                {
                    info.Display = 0;
                }
                
                if (info.ItemId > 0)
                {
                    info.Items = info.ItemId.ToString();
                }
                info.UserId = this.userOnlineInfo.UserId;
                info.Author = this.userOnlineInfo.UserName;
                if (string.IsNullOrEmpty(info.Source))
                {
                    info.Source = this.config.SiteName;
                }
                manage.updateDetail(info);
                if (info.DetailId > 0)
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        var contentInfo = new Entity.DetailContentInfo()
                        {
                            Title = info.Title,
                            Content = content,
                            OrderNo = 0,
                            DetailId = info.DetailId,
                            ContentId = cid,
                            ItemId = info.ItemId,
                            ChannelCode = info.ChannelCode,
                            InDate = Config.SiteConfig.getLocalTime()
                        };
                        manage.updateDetailContent(contentInfo);
                    }
                    if (!string.IsNullOrEmpty(pics))
                    {
                        //更新图库
                        JavaScriptSerializer jsHelper = new JavaScriptSerializer();
                        var gallerylist = jsHelper.Deserialize<List<Entity.DetailGalleryInfo>>(pics);
                        if (gallerylist != null)
                        {
                            manage.updateDetailGallery(gallerylist, info.DetailId);
                        }
                    }
                }
                if (info.Display != 1 && this.config.VerifyUserDetail)
                {
                    manage.Message = "发表成功，系统审核后将进行展示！";
                }
                else
                {
                    manage.Message = "发表成功";
                }

                return this.getResult(manage.Error, manage.Message);
            }
        }
        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult deleteDetail(int id)
        {
            if (id > 0)
            {
                using (var manage = new bitcms.Data.CMSManage())
                {
                    var detailInfo = manage.getDetailInfo(id);
                    if (detailInfo != null)
                    {
                        if (detailInfo.UserId == this.userOnlineInfo.UserId)
                        {
                            manage.deleteDetail(id);
                            return this.getResult(manage.Error, manage.Message);
                        }
                        else
                        {
                            return this.getResult(Entity.Error.错误, "非法操作");
                        }
                    }
                }
            }
            return this.getResult(Entity.Error.错误, "参数错误");
        }

        /// <summary>
        /// 获取文章点赞列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getViewPointList(string dids,string rids)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var list = manage.getViewPointList(this.userOnlineInfo.UserId, dids, rids);
                return this.getResult(manage.Error, manage.Message, list);
            }
        }
        /// <summary>
        /// 设置点赞
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult setViewPoint(int detailid = 0, int reviewid = 0, int agree = -1, int against = -1)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var num = manage.setViewPoint(this.userOnlineInfo.UserId, detailid, reviewid, agree, against);
                return this.getResult(manage.Error, manage.Message, new { num });
            }
        }


        /// <summary>
        /// 获取会员评论列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult getUserReviewList(Entity.PageInfo page, string code, int detailid, int replyid = 0, int userid = 0)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var list = manage.getReviewList(page, code, detailid, replyid, userid, 1);
                return this.getResult(manage.Error, manage.Message, list);
            }
        }
        /// <summary>
        /// 更新内容点击次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult updateDetailHitsNum(int id)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var num = manage.insertDetailHits(this.userOnlineInfo.UserOnlineId, this.userOnlineInfo.UserId, id);
                return this.getResult(manage.Error, manage.Message, new { num = num });
            }
        }
        /// <summary>
        /// 检查收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult checkfavorties(string code, int targetid)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var result = manage.checkFavorites(code, targetid);
                return this.getResult(manage.Error, manage.Message, result);
            }
        }
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addfavorties(string code, int targetid, string title, string link, string pic = null, string describe = null)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var num = manage.insertFavoritesInfo(new Entity.FavoritesInfo()
                {
                    FavoritesCode = code,
                    TargetId = targetid,
                    Title = title,
                    Link = link,
                    Pic = pic,
                    Describe = describe,
                    InDate = Config.SiteConfig.getLocalTime(),
                    UserId = this.userOnlineInfo.UserId

                });
                return this.getResult(manage.Error, manage.Message, num);
            }
        }
        #endregion

        #region 会员关注
        /// <summary>
        /// 检查会员关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult checkUserFollow(int userid, int followUserId)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                var result = manage.checkFollow(userid, followUserId);
                return this.getResult(manage.Error, manage.Message, result);
            }
        }
        /// <summary>
        /// 添加会员关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult addFollow(int userid, int followUserId)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                if (!manage.insertFollowInfo(userid, followUserId))
                {
                    manage.Error = Entity.Error.错误;
                    manage.Message = "已关注";
                }
                return this.getResult(manage.Error, manage.Message);
            }
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="followUserId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult cancelFollow(int userid, int followUserId)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {
                if (!manage.deleteFollowInfo(userid, followUserId))
                {
                    manage.Error = Entity.Error.错误;
                    manage.Message = "取消失败";
                }
                return this.getResult(manage.Error, manage.Message);
            }
        }
        #endregion

        #region 评论
        /// <summary>
        /// 更新评论点击次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult updateReviewHitsNum(int id)
        {
            using (var manage = new bitcms.Data.CMSManage())
            {

                var num = manage.updateReviewHitsNum(id);
                return this.getResult(manage.Error, manage.Message, new { num = num });
            }
        }

        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult updateReview(Entity.ReviewInfo info, string key, string verifykey)
        {
            if (this.config.EnabledReviewVerifykey)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "verifycode";
                }
                var code = Config.UserConfig.getVerifyCode(key);
                if (string.IsNullOrEmpty(verifykey) || code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误！");
                }
            }
            if (info != null && !string.IsNullOrEmpty(info.Content))
            {
                using (var manage = new bitcms.Data.CMSManage())
                {
                    if (this.config.VerifyReview)
                    {
                        info.Verify = 0;
                    }
                    else
                    {
                        info.Verify = 1;
                    }
                    info.UserId = this.userOnlineInfo.UserId;
                    manage.addReview(info);
                    if (info.Verify != 1)
                    {
                        manage.Message = "评论成功，系统审核后再进行展示！";
                    }
                    else
                    {
                        manage.Message = "评论成功";
                    }

                    return this.getResult(manage.Error, manage.Message);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "内容为空！");
            }
        }

        /// <summary>
        /// 更新回复
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult updateReply(int replyid, string content, int detailid = 0, string key = null, string verifykey = null)
        {
            if (this.config.EnabledReviewVerifykey)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = "verifycode";
                }
                var code = Config.UserConfig.getVerifyCode(key);
                if (string.IsNullOrEmpty(verifykey) || code == null || !code.Code.Equals(verifykey.ToLower()))
                {
                    return this.getResult(Entity.Error.错误, "验证码错误！");
                }
            }
            if (replyid > 0 && !string.IsNullOrEmpty(content))
            {
                using (var manage = new bitcms.Data.CMSManage())
                {
                    manage.addReply(replyid, content, this.userOnlineInfo.UserId, this.config.VerifyReview ? 0 : 1);
                    if (!this.config.VerifyReview)
                    {
                        manage.Message = "回复成功";
                    }
                    else
                    {
                        manage.Message = "回复成功，系统审核后再进行展示！";
                    }

                    return this.getResult(manage.Error, manage.Message);
                }
            }
            else
            {
                return this.getResult(Entity.Error.错误, "内容为空！");
            }
        }
        #endregion
    }



}