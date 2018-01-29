using System;
using bitcms.Entity;

namespace bitcms.DataProvider
{
    public abstract class DataBase : IDisposable
    {
        #region 错误消息
        /// <summary>
        /// 错误代码
        /// </summary>
        public Entity.Error Error { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region database
        /// <summary>
        /// 数据库
        /// </summary>
        protected DbContext dbContext { get; set; }

        /// <summary>
        /// 网站配置
        /// </summary>
        protected Entity.SiteConfigInfo siteConfig  { get; set; }

        /// <summary>
        /// 继承类构造函数，请传值true
        /// </summary>
        /// <param name="isinherit"></param>
        public DataBase(bool isinherit)
        {
            if (!isinherit)
            {
                var type = ContextType.read;
                if (Common.Fetch.isPost())//post请求
                {
                    type = ContextType.update;
                }

                dbContext = new DbContext(type);
            }
            siteConfig = Config.SiteConfig.load();
        }

        #endregion

        #region 析构函数
        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
        #endregion

        #region 公共方法

        #endregion
    }
}
