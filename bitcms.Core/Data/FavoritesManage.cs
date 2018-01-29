using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bitcms.DataProvider;
using bitcms.Entity;

namespace bitcms.Data
{
    public partial class CMSManage : bitcms.DataProvider.DataBase
    {
        /// <summary>
        /// 插入收藏
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int insertFavoritesInfo(Entity.FavoritesInfo info)
        {
            if (!string.IsNullOrEmpty(info.FavoritesCode))
            {
                info.FavoritesCode = info.FavoritesCode.ToLower();
            }
            this.dbContext.Favorites.Add(info);

            var channelinfo = this.getDetailChannelInfo(info.FavoritesCode);
            var num = 0;
            if (channelinfo != null)
            {
                num = this.updateDetailFollowNum(info.TargetId);
            }
            this.dbContext.SaveChanges();
            return num;
        }
        /// <summary>
        /// 检查是否已经收藏
        /// </summary>
        /// <param name="favoritescode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool checkFavorites(string favoritescode, int targetid)
        {
            if (!string.IsNullOrEmpty(favoritescode))
            {
                favoritescode = favoritescode.ToLower();
            }
            return this.dbContext.Favorites.Count(g => g.FavoritesCode.Equals(favoritescode) && g.TargetId == targetid) > 0;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        public List<Entity.FavoritesInfo> getFavoritesList(PageInfo page, int userid, string favoritescode)
        {
            var lambda = PredicateExtensions.True<Entity.FavoritesInfo>();

            if (userid > 0)
                lambda = lambda.And(g => g.UserId == userid);
            if (!string.IsNullOrEmpty(favoritescode))
            {
                lambda = lambda.And(g => g.FavoritesCode.Equals(favoritescode));
            }
            if (!string.IsNullOrEmpty(page.Key))
            {
                lambda = lambda.And(g => g.Title.Contains(page.Key));
            }

            if (page.TotalCount == 0)
            {
                var list = this.dbContext.Favorites.Where(lambda).OrderByDescending(g => g.FavoritesId);
                page.TotalCount = list.Count();
                return list.Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }
            else
            {
                return this.dbContext.Favorites.Where(lambda).OrderByDescending(g => g.FavoritesId).Skip(page.PageSize * (page.PageNumber - 1)).Take(page.PageSize).ToList();
            }

        }
    }
}
