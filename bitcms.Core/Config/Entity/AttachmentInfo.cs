
namespace bitcms.Entity
{
    /// <summary>
    /// 附件实体类
    /// </summary>
    public class AttachmentInfo
    {
        #region Model
        private string _title;
        private string _path;
        private int _size;
        private int _width;
        private int _height;

        /// <summary>
        /// 标题 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 路径 
        /// </summary>
        public string Path
        {
            set { _path = value; }
            get { return _path; }
        }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        #endregion Model
    }
}
