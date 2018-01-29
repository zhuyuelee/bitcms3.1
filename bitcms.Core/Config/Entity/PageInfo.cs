
namespace bitcms.Entity
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageInfo
    {
        #region 分页数据
        private int pagesize = 10;
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }
        private int pagenumber = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNumber
        {
            get
            {
                if (pagenumber < 1) return 1;
                else return pagenumber;
            }
            set { pagenumber = value; }
        }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                int pagecount = 1;
                if (this.TotalCount > this.pagesize)
                {
                    pagecount = this.TotalCount / this.pagesize;
                    if (this.TotalCount % this.pagesize > 0)
                        pagecount++;
                }
                return pagecount;
            }
        }
        /// <summary>
        /// 排序0正序  1倒序
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderField { get; set; }
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key { get; set; }
        #endregion
    }
}
