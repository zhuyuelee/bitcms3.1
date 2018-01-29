using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using bitcms.Common;
using MySql.Data.MySqlClient;

namespace bitcms.web.Areas.Admin
{
    public class InstallController : UI.BaseController
    {
        /// <summary>
        /// 读数据库链接
        /// </summary>
        const string readConn = "bitcmsReadConn";
        /// <summary>
        /// 写数据库链接
        /// </summary>
        const string updateConn = "bitcmsUpdateConn";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Fetch.fileExist(Fetch.getMapPath("/install/lock.ini")))
            {
                object action = null;
                if (filterContext.RouteData.Values.TryGetValue("action", out action))
                {
                    if (action != null && action.ToString().ToLower() != "succeed")
                    {
                        ViewBag.Title = "安装已经锁定";
                        ViewBag.Message = "安装已经锁定, 请删除install文件夹下的lock.ini和数据库配置再进行安装！";
                        filterContext.Result = this.Content(this.getSystemTemplate("error.cshtml"));
                        return;
                    }
                }
               
            }
            base.OnActionExecuting(filterContext);
        }
        //
        // GET: /Admin/Install/
        public ActionResult Index()
        {
            return View();
        }

        #region 第二步
        //
        // GET: /Admin/Step2/
        public ActionResult Step2()
        {
            var powerlist = new Dictionary<string, bool>();
            powerlist.Add("web.config", checkWebConfig());
            powerlist.Add("App_Data", checkDirectory("App_Data"));
            powerlist.Add("cache", checkDirectory("cache"));
            powerlist.Add("config", checkDirectory("config"));
            powerlist.Add("install", checkDirectory("install"));
            powerlist.Add("upload", checkDirectory("upload"));
            return View(powerlist);
        }

        /// <summary>
        /// 检查web.config
        /// </summary>
        /// <returns></returns>
        private bool checkWebConfig()
        {
            try
            {
                var webconfig = WebConfigurationManager.OpenWebConfiguration("~");
                var key = Guid.NewGuid().ToString("N");
                if (webconfig.ConnectionStrings.ConnectionStrings[key] != null)
                {
                    var settings = webconfig.ConnectionStrings.ConnectionStrings[key];
                    webconfig.ConnectionStrings.ConnectionStrings.Remove(settings);
                    webconfig.ConnectionStrings.ConnectionStrings.Add(settings);

                }
                else
                {
                    var settings = new ConnectionStringSettings();
                    settings.ConnectionString = "test";
                    settings.Name = key;
                    webconfig.ConnectionStrings.ConnectionStrings.Add(settings);
                    webconfig.ConnectionStrings.ConnectionStrings.Remove(settings);
                }
                webconfig.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 检查文件夹可读写
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool checkDirectory(string path)
        {
            var serverpath = Fetch.getMapPath("/" + path);
            if (!Directory.Exists(serverpath))
            {
                Directory.CreateDirectory(serverpath);
            }
            try
            {
                var filename = Guid.NewGuid().ToString("N") + ".ini";
                using (FileStream fs = new FileStream(serverpath + filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }

                System.IO.File.Delete(serverpath + filename);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 第三步
        //
        // GET: /Admin/Step3/
        public ActionResult Step3()
        {
            return View();
        }


        /// <summary>
        /// 检查数据库连接有效性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult checkDatabase(string server, uint port, string database, string userid, string password)
        {
            var result = this.checkMySqlConnection(server, port, database, userid, password);
            return this.getResult(result ? Entity.Error.请求成功 : Entity.Error.错误, "");
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult createtable(string server, uint port, string database, string userid, string password, string characterset, int drop = 0)
        {
            var filelist = new List<string>();
            var resultlist = new List<string>();
            if (drop == 1)
            {
                filelist.Add("drop.sql");
            }
            filelist.Add("table.sql");
            filelist.Add("data.sql");
            filelist.Add("view.sql");
            foreach (var file in filelist)
            {
                resultlist.Add(this.executeSql(file, server, port, database, userid, password, characterset));
            }
            return this.getResult(Entity.Error.请求成功, "请求成功", resultlist);
        }

        /// <summary>
        /// 插入测试数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult insertTestData(string server, uint port, string database, string userid, string password, string characterset, int drop = 0)
        {
            var resultlist = new List<string>();
            resultlist.Add(this.executeSql("testdata.sql", server, port, database, userid, password, characterset));
            return this.getResult(Entity.Error.请求成功, "请求成功", resultlist);
        }
        /// <summary>
        /// 网站配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult systemConfig(string server, uint port, string database, string userid, string password, string characterset, string sitename, string sitedomain, string sitepath)
        {
            var config = Config.SiteConfig.load();
            config.SiteCode = Guid.NewGuid().ToString("N");
            config.SiteDomain = sitedomain;
            config.SitePath = sitepath;
            config.SiteName = sitename;
            Config.SiteConfig.save(config);
            this.config = config;
            saveWebConfig(server, port, database, userid, password, characterset);

            return this.getResult(Entity.Error.请求成功, "请求成功");
        }
        /// <summary>
        /// 网站配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult initAdminUser(string username, string adminpassword)
        {
            using (var manage = new Data.CMSManage())
            {
                var userinfo = new Entity.UserInfo();
                userinfo.UserName = username;
                var roleinfo = manage.getRoleByReadPower("admin", 0);
                if (roleinfo != null)
                {
                    userinfo.RoleId = roleinfo.RoleId;
                }
                else
                {
                    userinfo.RoleId = 1;
                }
                userinfo.InDate = DateTime.Now;
                userinfo.IP = Fetch.getClientIP();
                userinfo.VerifyMember = 1;
                userinfo.ComeFrom = "系统安装";
                userinfo.LastLandDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                userinfo.LockUser = 0;
                manage.updateUser(userinfo);
                if (userinfo.UserId > 0)//更新密码
                {
                    manage.updatePassword(userinfo.UserId, Entity.passwordType.manage, adminpassword);
                    manage.updatePassword(userinfo.UserId, Entity.passwordType.user, adminpassword);
                }
            }

            System.IO.File.WriteAllText(Fetch.getMapPath("/install/lock.ini"), "installdate:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), System.Text.Encoding.UTF8);

            return this.getResult(Entity.Error.请求成功, "请求成功");
        }

        /// <summary>
        /// 保存数据库配置
        /// </summary>
        /// <returns></returns>
        private bool saveWebConfig(string server, uint port, string database, string userid, string password, string characterset)
        {
            try
            {
                var webconfig = WebConfigurationManager.OpenWebConfiguration("~");

                MySqlConnectionStringBuilder connSbString = new MySqlConnectionStringBuilder();
                connSbString.Server = server;
                connSbString.Port = port;
                connSbString.UserID = userid;
                connSbString.Database = database;
                connSbString.Password = password;
                connSbString.CharacterSet = characterset;
                var connstring = connSbString.GetConnectionString(true);

                var readsettings = new ConnectionStringSettings();
                readsettings.ConnectionString = connstring;
                readsettings.Name = readConn;
                readsettings.ProviderName = "MySql.Data.MySqlClient";
                if (webconfig.ConnectionStrings.ConnectionStrings[readConn] != null)
                {
                    webconfig.ConnectionStrings.ConnectionStrings.Remove(readConn);
                }
                webconfig.ConnectionStrings.ConnectionStrings.Add(readsettings);

                var updatesettings = new ConnectionStringSettings();
                updatesettings.ConnectionString = connstring;
                updatesettings.Name = updateConn;
                updatesettings.ProviderName = "MySql.Data.MySqlClient";

                if (webconfig.ConnectionStrings.ConnectionStrings[updateConn] != null)
                {
                    webconfig.ConnectionStrings.ConnectionStrings.Remove(updateConn);
                }
                webconfig.ConnectionStrings.ConnectionStrings.Add(updatesettings);

                webconfig.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <returns></returns>
        private string executeSql(string file, string server, uint port, string database, string userid, string password, string characterset)
        {
            MySqlConnection conn = null;
            try
            {
                var serverfile = Fetch.getMapPath(string.Format("/install/sql/{0}", file));
                var sql = Fetch.getFile(serverfile);
                if (!string.IsNullOrEmpty(sql))
                {
                    conn = mySqlConnection(server, port, database, userid, password, characterset);
                    conn.Open();
                    try
                    {
                        MySqlScript script = new MySqlScript(conn, sql);
                        if (script.Execute() > 0)
                        {
                            return string.Format("{0}执行成功", file);
                        }
                        else
                        {
                            return string.Format("{0}文件执行无结果", file);
                        }
                    }
                    catch (Exception)
                    {
                        return string.Format("{0}文件执行错误", file);
                    }
                }
                else
                {
                    return string.Format("{0}文件不存在", file);
                }
            }
            catch (Exception)
            {
                return "请检查数据库服务器的有效性";
            }
            finally
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Dispose();
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 检查数据库连接有效性
        /// </summary>
        /// <returns></returns>
        private bool checkMySqlConnection(string server, uint port, string database, string userid, string password)
        {

            MySqlConnection conn = null;
            try
            {
                conn = mySqlConnection(server, port, database, userid, password, "");
                conn.Open();
                return conn.Ping();

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Dispose();
                    conn.Close();
                }
            }

        }
        /// <summary>
        /// MySqlConnection
        /// </summary>
        /// <returns></returns>
        private MySqlConnection mySqlConnection(string server, uint port, string database, string userid, string password, string characterset)
        {
            MySqlConnectionStringBuilder connSbString = new MySqlConnectionStringBuilder();
            connSbString.Server = server;
            connSbString.Port = port;
            connSbString.UserID = userid;
            connSbString.Database = database;
            connSbString.Password = password;
            connSbString.CharacterSet = characterset;
            try
            {
                return new MySqlConnection(connSbString.GetConnectionString(true));
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 第四步
        //
        // GET: /Admin/Step4/
        public ActionResult succeed(string username)
        {
            this.setViewBag(this.config, this.userOnlineInfo);
            ViewBag.username = username;
            return View();
        }

        #endregion
    }
}