using System;
using System.Xml.Serialization;

namespace bitcms.Entity
{
    #region UploadConfig实体
    /// <summary>
    /// UploadConfig设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    [XmlRoot("Root")]
    public class UploadConfigInfo
    {

        #region  附件设置
        private string attachExtension = ".jpe|.jpeg|.jpg|.png|.gif|.bmp|.swf|.zip|.rar|.flv|.mp4|.mp3|.wma|.mid";
        /// <summary>
        /// 附件类型
        /// </summary>
        public string AttachExtension
        {
            get { return this.attachExtension; }
            set { this.attachExtension = value; }
        }

        private string multimediaExtension = ".jpe|.jpeg|.jpg|.png|.gif|.bmp|.swf|.flv|.mp4|.mp3|.wma|.mid";
        /// <summary>
        /// 多媒体附件类型
        /// </summary>
        public string MultimediaExtension
        {
            get { return this.multimediaExtension; }
            set { this.multimediaExtension = value; }
        }


        private string attachPath = "upload";
        /// <summary>
        /// 附件路径
        /// </summary>
        public string AttachPath
        {
            get { return this.attachPath.EndsWith("/") ? this.attachPath : this.attachPath + "/"; }
            set { this.attachPath = value; }
        }


        private int attachMaxSize = 1024 * 1024;
        /// <summary>
        /// 附件大小限制 字节 
        /// </summary>
        public int AttachMaxSize
        {
            get { return this.attachMaxSize; }
            set { this.attachMaxSize = value; }
        }

        private int watermarkType = -1;
        /// <summary>
        /// 水印类型 -1 不加水印 0=文字 1=图片
        /// </summary>
        public int WatermarkType
        {
            get { return this.watermarkType; }
            set { this.watermarkType = value; }
        }

        private string watermarkPic = "";
        /// <summary>
        /// 使用的水印图片的名称
        /// </summary>
        public string WatermarkPic
        {
            get { return this.watermarkPic; }
            set { this.watermarkPic = value; }
        }


        /// <summary>
        /// 图片附件添加水印 0=左上 1=中上 2=右上 3=左中 ... 8=右下
        /// </summary>
        public int WatermarkStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 图片水印透明度 取值范围1--10 (10为不透明)
        /// </summary>
        public int WatermarkTransparency
        {
            get;
            set;
        }

        private string watermarkText = "bitcms.net";
        /// <summary>
        /// 文字水印的内容
        /// </summary>
        public string WatermarkText
        {
            get { return this.watermarkText; }
            set { this.watermarkText = value; }
        }

        private string watermarkFont = "Arial";
        /// <summary>
        /// 添加文字水印的字体
        /// </summary>
        public string WatermarkFont
        {
            get { return this.watermarkFont; }
            set { this.watermarkFont = value; }
        }

        private int watermarkFontSize = 12;
        /// <summary>
        /// 文字水印的大小(像素)
        /// </summary>
        public int WatermarkFontSize
        {
            get { return this.watermarkFontSize; }
            set { this.watermarkFontSize = value; }
        }

        private int attachImgMaxHeight = 768;
        /// <summary>
        /// 附件图片最大高度 0为不受限制
        /// </summary>
        public int AttachImgMaxHeight
        {
            get { return this.attachImgMaxHeight; }
            set { this.attachImgMaxHeight = value; }
        }


        private int attachImgMaxWidth = 1280;
        /// <summary>
        /// 附件图片最大宽度 0为不受限制
        /// </summary>
        public int AttachImgMaxWidth
        {
            get { return this.attachImgMaxWidth; }
            set { this.attachImgMaxWidth = value; }
        }
        #endregion
    }
    #endregion
}
