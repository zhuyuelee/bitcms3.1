using System.Data.Entity;

namespace bitcms.DataProvider
{
    public class DbContext : System.Data.Entity.DbContext
    {
        #region 数据库设置
        /// <summary>
        /// 读数据库链接
        /// </summary>
        internal const string readConn = "bitcmsReadConn";
        /// <summary>
        /// 写数据库链接
        /// </summary>
        internal const string updateConn = "bitcmsUpdateConn";
        /// <summary>
        /// 获取链接字符
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string getConn(Entity.ContextType type)
        {
            var connectionString = readConn;
            if (type == Entity.ContextType.update)
            {
                connectionString = updateConn;
            }
            return string.Format("name={0}", connectionString);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        public DbContext(Entity.ContextType type)
            : base(getConn(type)) { }
        #endregion

        #region 系统相关
        /// <summary>
        /// 后台管理目录
        /// </summary>
        public DbSet<Entity.AdminMenuInfo> AdminMenu { get; set; }

        /// <summary>
        /// 快捷菜单
        /// </summary>
        public DbSet<Entity.AdminMenuShortcutInfo> AdminMenuShortcut { get; set; }
        /// <summary>
        /// 路由信息
        /// </summary>
        public DbSet<Entity.MapRouteInfo> MapRoute { get; set; }

        /// <summary>
        /// 字典
        /// </summary>
        public DbSet<Entity.DictionaryInfo> Dictionary { get; set; }

        /// <summary>
        /// 字典key
        /// </summary>
        public DbSet<Entity.DictionaryKeyInfo> DictionaryKey { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public DbSet<Entity.ModuleInfo> Module { get; set; }
        #endregion

        #region 会员相关
        /// <summary>
        /// 会员信息
        /// </summary>
        public DbSet<Entity.UserInfo> User { get; set; }
        /// <summary>
        /// 会员信息
        /// </summary>
        public DbSet<Entity.UserViewInfo> UserView { get; set; }
        /// <summary>
        /// 会员密码
        /// </summary>
        public DbSet<Entity.UserPasswordInfo> UserPassword { get; set; }
        /// <summary>
        /// 会员账号绑定
        /// </summary>
        public DbSet<Entity.UserBindInfo> UserBind { get; set; }
        /// <summary>
        /// 推荐会员 
        /// </summary>
        public DbSet<Entity.RefereeInfo> Referee { get; set; }

        /// <summary>
        /// 积分事件
        /// </summary>
        public DbSet<Entity.ScoreEventInfo> ScoreEvent { get; set; }

        /// <summary>
        /// 会员积分记录
        /// </summary>
        public DbSet<Entity.ScoreLogInfo> ScoreLog { get; set; }

        /// <summary>
        /// 会员角色
        /// </summary>
        public DbSet<Entity.RoleInfo> Role { get; set; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public DbSet<Entity.RolePowerInfo> RolePower { get; set; }
        #endregion

        #region 内容相关
        /// <summary>
        /// 城市
        /// </summary>
        public DbSet<Entity.CityInfo> City { get; set; }

        /// <summary>
        /// 广告
        /// </summary>
        public DbSet<Entity.AdsInfo> Ads { get; set; }

        /// <summary>
        /// 广告详情
        /// </summary>
        public DbSet<Entity.AdsDetailInfo> AdsDetail { get; set; }

        /// <summary>
        /// 内容频道
        /// </summary>
        public DbSet<Entity.DetailChannelInfo> DetailChannel { get; set; }

        /// <summary>
        /// 内容栏目
        /// </summary>
        public DbSet<Entity.ItemInfo> Item { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public DbSet<Entity.DetailInfo> Detail { get; set; }


        /// <summary>
        /// 内容详情
        /// </summary>
        public DbSet<Entity.DetailContentInfo> DetailContent { get; set; }

        /// <summary>
        /// 内容图库
        /// </summary>
        public DbSet<Entity.DetailGalleryInfo> DetailGallery { get; set; }
        /// <summary>
        /// 内容点击
        /// </summary>
        public DbSet<Entity.DetailHitsInfo> DetailHits { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<Entity.ReviewInfo> Review { get; set; }
        /// <summary>
        /// 会员评论
        /// </summary>
        public DbSet<Entity.UserReviewInfo> UserReview { get; set; }
        /// <summary>
        /// 内容评论
        /// </summary>
        public DbSet<Entity.DetailReviewInfo> DetailReview { get; set; }
        /// <summary>
        /// 内容点赞
        /// </summary>
        public DbSet<Entity.ViewPointInfo> ViewPoint { get; set; }
        /// <summary>
        /// 收藏
        /// </summary>
        public DbSet<Entity.FavoritesInfo> Favorites { get; set; }
        /// <summary>
        /// 关注
        /// </summary>
        public DbSet<Entity.FollowInfo> Follow { get; set; }
        #endregion
    }
}
