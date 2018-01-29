using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using bitcms.Common;
using bitcms.Ueditor;
using bitcms.UI;

namespace bitcms.Web.Controllers
{
    public class ToolsController : BaseActionController
    {

        #region ActionExecuting
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isAdmin = false;
            if (this.userOnlineInfo.AdminOnline)
            {
                object action = null;
                if (filterContext.RouteData.Values.TryGetValue("action", out action))
                {
                    var adminAction = "|upload|ueditorupload|";
                    if (action != null && adminAction.IndexOf("|" + action.ToString().ToLower() + "|") > -1)
                    {
                        //管理员上传
                        isAdmin = true;
                    }
                }
            }
            if (!isAdmin)
            {
                base.OnActionExecuting(filterContext);
            }
        }
        #endregion
        public ToolsController()
            : base(true) { }
        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult upload(string folder, int watermark)
        {
            var files = this.Request.Files;
            var path = "";
            Entity.AttachmentInfo attachmentInfo = null;
            if (files.Count > 0)
            {
                try
                {
                    Common.Upload uploadFile = new Common.Upload();
                    attachmentInfo = uploadFile.save(folder, files.Get(0), watermark == 1);
                    path = attachmentInfo.Path;
                }
                catch (Exception ex)
                {
                    return getResult(bitcms.Entity.Error.错误, ex.ToString());
                }
            }
            return getResult(bitcms.Entity.Error.请求成功, "上传成功", path);

        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [Route("tools/verifyKey/{key?}/{width?}/{height?}")]
        public FileResult VerifyKey(string key, int width = 0, int height = 0)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = "verifycode";
            }
            Entity.VerifyCode code = Config.UserConfig.getVerifyCode(key, false);
            if (code == null)
            {
                code = Config.UserConfig.setVerifyCode(key, Utils.random(4));

            }

            var image = UI.VerifyKey.getVerifyKey(code.Code, width, height);
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            image.Dispose();
            return File(stream.ToArray(), "image/PNG");
        }

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public JsonResult getCity(int cityid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getCityList(cityid);
                return getResult(manage.Error, manage.Message, list);
            }
        }
        #region 附件下载
        /// <summary>
        /// 附件下载
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        [Route("tools/getattachment/{id}")]
        public ActionResult getAttachment(int id)
        {
            var error = Entity.Error.请求成功;
            var message = "";
            if (id > 0)
            {
                using (var manage = new Data.CMSManage())
                {
                    var galleryInfo = manage.getDetailGalleryInfo(id);
                    if (galleryInfo != null && galleryInfo.GalleryType == Entity.GalleryType.attachment && !string.IsNullOrEmpty(galleryInfo.Path))
                    {
                        if (galleryInfo.ReadPower >= 0)//下载权限
                        {
                            var readpower = galleryInfo.ReadPower;
                            if (readpower == 0)//继承
                            {
                                var detailinfo = manage.getDetailInfo(galleryInfo.DetailId);
                                if (detailinfo != null)
                                {
                                    readpower = detailinfo.ReadPower;
                                    if (readpower == 0)
                                    {
                                        readpower = manage.getItemReadPower(detailinfo.ItemId);
                                    }
                                }
                            }

                            if (readpower > 0)
                            {
                                if (this.userOnlineInfo.UserId <= 0)
                                {
                                    error = Entity.Error.登录超时;
                                    message = "登录超时,无权下载";
                                }
                                else
                                {
                                    var roleinfo = manage.getRoleInfo(this.userOnlineInfo.UserInfo.RoleId);
                                    if (roleinfo == null && roleinfo.ReadPower < readpower)
                                    {
                                        error = Entity.Error.无查看权限;
                                        message = "会员级别过低，无权下载!";
                                    }
                                }
                            }
                        }
                        if (error == Entity.Error.请求成功)
                        {
                            var path = galleryInfo.Path.ToLower();
                            if (path.StartsWith(this.config.SiteDomain))//去本地域名
                            {
                                path = path.Substring(this.config.SiteDomain.Length - 1);
                            }
                            var islocal = false;
                            if (path.StartsWith(this.config.SitePath))
                            {
                                if (!Fetch.fileExist(Fetch.getMapPath(path)))
                                {
                                    error = Entity.Error.错误;
                                    message = "文件不存在,下载失败!";
                                }
                                islocal = true;
                            }
                            if (error == Entity.Error.请求成功)
                            {
                                galleryInfo.HitsNum++;
                                manage.updateDetailGallery(galleryInfo);
                                
                                if (islocal)
                                    return File(path, Path.GetExtension(path), Path.GetFileName(path));
                                else
                                    return Redirect(path);
                            }
                        }
                    }
                }
            }
            else
            {
                error = Entity.Error.错误;
                message = "参数错误，下载失败!";
            }
            ViewBag.Title = error.ToString();
            ViewBag.Message = message;
            return PartialView("error");
        }

        public RedirectResult MyProperty { get; set; }
        #endregion

        #region 图片处理
        /// <summary>
        /// 获取附件图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [Route("tools/getimg/{id}/{width?}/{height?}")]
        public ActionResult getImg(int id, int width = 0, int height = 0)
        {
            string pic = null;
            if (id > 0)
            {
                using (var manage = new Data.CMSManage())
                {
                    var galleryInfo = manage.getDetailGalleryInfo(id);
                    if (galleryInfo != null && galleryInfo.GalleryType == Entity.GalleryType.picture)
                    {
                        pic = galleryInfo.Path;
                        galleryInfo.HitsNum++;
                        manage.updateDetailGallery(galleryInfo);
                    }
                }
            }

            try
            {
                pic = cutImg(pic, null, width, height);
                if (string.IsNullOrEmpty(pic))
                {
                    pic = "/images/pic.png";
                }
                return File(pic, Path.GetExtension(pic));
            }
            catch (Exception)
            {
                return File("/images/pic.png", ".png");
            }

        }

        /// <summary>
        /// 获取头像
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [Route("tools/getavatar/{userid}/{width?}/{height?}")]
        public ActionResult getavatar(int userid, int width = 0, int height = 0)
        {
            string avatar = null;

            if (userid > 0)
            {
                using (var manage = new Data.CMSManage())
                {
                    var userinfo = manage.getUserInfo(userid);
                    if (userinfo != null)
                    {
                        avatar = userinfo.Avatar;
                    }
                }
            }

            try
            {
                avatar = cutImg(avatar, null, width, height);

                if (string.IsNullOrEmpty(avatar))
                {
                    avatar = "/images/noavatar_big.gif";
                }

                return File(avatar, Path.GetExtension(avatar));
            }
            catch (Exception)
            {
                return File("/images/noavatar_big.gif", ".gif");
            }
        }
        /// <summary>
        /// 裁切图片
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private string cutImg(string img, string newpic, int width, int height)
        {
            if (string.IsNullOrEmpty(img))
            {
                return null;
            }

            var ext = Path.GetExtension(img);//后缀

            if (width > 0 || height > 0)
            {
                if (string.IsNullOrEmpty(newpic))
                {
                    newpic = img.Replace(ext, string.Format("_{0}-{1}{2}", width, height, ext));//新图片地址
                }
                if (Fetch.fileExist(Fetch.getMapPath(newpic)))//小图存在
                {
                    return newpic;
                }
                else if (!Fetch.fileExist(Fetch.getMapPath(img)))//原图不存在
                {
                    return null;
                }
                else//原图存在 小图片不存在，建立
                {
                    img = Upload.setThumbnailPic(img, width, height, newpic);
                }
            }

            return img;
        }
        #endregion

        #region 百度编辑上传
        // GET: /Ueditors/
        public ActionResult ueditorupload()
        {
            Handler action = null;
            switch (this.HttpContext.Request["action"])
            {
                case "config":
                    action = new ConfigHandler(this.HttpContext);
                    break;
                case "uploadimage":
                    action = new UploadHandler(this.HttpContext, new UploadConfig()
                    {
                        AllowExtensions = bitcms.Ueditor.Config.GetStringList("imageAllowFiles"),
                        UploadFieldName = bitcms.Ueditor.Config.GetString("imageFieldName"),
                        Folder = "image"
                    });
                    break;
                case "uploadscrawl":
                    action = new UploadHandler(this.HttpContext, new UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        UploadFieldName = bitcms.Ueditor.Config.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png",
                        Folder = "scrawl"
                    });
                    break;
                case "uploadvideo":
                    action = new UploadHandler(this.HttpContext, new UploadConfig()
                    {
                        AllowExtensions = bitcms.Ueditor.Config.GetStringList("videoAllowFiles"),
                        UploadFieldName = bitcms.Ueditor.Config.GetString("videoFieldName"),
                        Folder = "video"
                    });
                    break;
                case "uploadfile":
                    action = new UploadHandler(this.HttpContext, new UploadConfig()
                    {
                        AllowExtensions = bitcms.Ueditor.Config.GetStringList("fileAllowFiles"),
                        UploadFieldName = bitcms.Ueditor.Config.GetString("fileFieldName"),
                        Folder = "file"
                    });
                    break;
                case "listimage":
                    action = new ListFileManager(this.HttpContext, bitcms.Ueditor.Config.GetString("imageManagerListPath"), bitcms.Ueditor.Config.GetStringList("imageManagerAllowFiles"));
                    break;
                case "listfile":
                    action = new ListFileManager(this.HttpContext, bitcms.Ueditor.Config.GetString("fileManagerListPath"), bitcms.Ueditor.Config.GetStringList("fileManagerAllowFiles"));
                    break;
                case "catchimage":
                    action = new CrawlerHandler(this.HttpContext);
                    break;
                default:
                    action = new NotSupportedHandler(this.HttpContext);
                    break;
            }
            return Content(action.Process());
        }
        #endregion
    }
}