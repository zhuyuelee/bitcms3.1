using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace bitcms.Common
{
    /// <summary>
    /// 带有缩略图的图片上传
    /// </summary>
    public class Upload
    {
        /// <summary>
        /// 上传配置
        /// </summary>
        private Entity.UploadConfigInfo config = null;

        /// <summary>
        /// 添加水印
        /// </summary>
        private bool watermark = true;


        public Upload()
        {
            config = Config.UploadConfig.load();
        }

        #region 上传操作

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public Entity.AttachmentInfo save(HttpPostedFileBase file)
        {
            return save("", file, watermark);
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        public Entity.AttachmentInfo save(string folder, HttpPostedFileBase file, bool watermark)
        {
            this.watermark = watermark;
            /// 文件文件设置
            Entity.AttachmentInfo attachInfo = new Entity.AttachmentInfo();
            string fileExt = Path.GetExtension(file.FileName).ToLower();//扩展名
            string turnPath = string.Empty;

            if (config.AttachExtension.IndexOf(fileExt) == -1)
            {
                throw new Exception("文件类型错误！不允许的文件类型" + fileExt);
            }
            else if (file.ContentLength < 10 || file.ContentLength > config.AttachMaxSize)
            {
                throw new Exception("文件选择有误，请上传大于1k，小于" + config.AttachMaxSize / 1024 + "K 的文件！");
            }
            else
            {
                attachInfo.Size = file.ContentLength / 1024;
                if (attachInfo.Size < 0)
                    attachInfo.Size = 1;
                attachInfo.Title = Common.Utils.trimEnd(file.FileName, fileExt);

                string filePath = this.getFilePath(folder);

                string fileName = this.getFileName(fileExt);//文件名

               

                string fileExtension = ".jpe|.jpeg|.jpg|.png|.gif|.bmp";
                if (fileExtension.IndexOf(fileExt) > -1)//图片类型
                {
                    Image upImg = Image.FromStream(file.InputStream);

                    attachInfo.Width = upImg.Width;
                    attachInfo.Height = upImg.Height;

                    saveImage(upImg, fileName, filePath);
                    upImg.Dispose();//销毁对象
                }
                else//保存文件
                {
                    string serverPath = Fetch.getMapPath(filePath);
                    if (!Directory.Exists(serverPath))//创建目录
                        Directory.CreateDirectory(serverPath);

                    file.SaveAs(serverPath + fileName);
                }

                attachInfo.Path = filePath + fileName;
            }
            return attachInfo;
        }

        public string getFileName(string ext)
        {
          return  string.Format("{0}-{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss").ToString(), DateTime.Now.AddMilliseconds(100).ToFileTime(), ext);
        }
        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <returns></returns>
        public string getFilePath(string folder)
        {
            var uploadPath = this.config.AttachPath;
            if (!uploadPath.StartsWith("/"))
            {
                uploadPath = Config.SiteConfig.getSitePath() + uploadPath;
            }

            if (!string.IsNullOrEmpty(folder))//上传目录
            {
                if (uploadPath.EndsWith("/"))
                    uploadPath += folder;
                else
                    uploadPath += "/" + folder;
            }
            if (!uploadPath.EndsWith("/"))
                uploadPath += "/";

           return string.Format("{0}{1}", uploadPath, DateTime.Now.ToString("yyyy/MMdd/"));//日期路径
        }
        
        /// <summary>
        /// 保存到图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="upImg"></param>
        public string saveImage(Image upImg, string fileName, string path)
        {
            Size imgSize = upImg.Size;
            var ext = Path.GetExtension(fileName).ToLower();
            if (!ext.Equals(".gif"))//非gif
            {
                if (config.AttachImgMaxHeight > 0)//最大宽高限制
                {
                    if (imgSize.Width > config.AttachImgMaxWidth && imgSize.Height > config.AttachImgMaxHeight)//最大限制尺寸
                    {
                        imgSize = resizeImage(imgSize.Width, imgSize.Height, config.AttachImgMaxWidth, config.AttachImgMaxHeight);//设置新尺寸
                        upImg = new Bitmap(upImg, imgSize);
                    }
                }

                if (this.watermark)//过小图片不加水印
                {
                    setWatermark(upImg, config);
                }
            }
           
            var serverPath = Fetch.getMapPath(path);
            if (!Directory.Exists(serverPath))//创建目录
                Directory.CreateDirectory(serverPath);

            upImg.Save(serverPath + fileName);//保存图
            upImg.Dispose();
            return path + fileName;
        }

        #endregion

        #region 缩略图处理
        /// <summary>
        /// 缩略图处理
        /// </summary>
        /// <param name="artwork"></param>
        /// <param name="thumbnail"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        public static string setThumbnailPic(string artwork, int width, int height, string thumbnail = null)
        {
            string localPath = Fetch.getMapPath(artwork);
            if (Fetch.fileExist(localPath))
            {
                Bitmap img = new Bitmap(localPath);
                if (img.Width <= width && img.Height <= height)//原图片要生成的缩略图还小
                {
                    img.Dispose();
                    return artwork;
                }
                else
                {
                    Image thumImg = getAttachThumbnail(img, width, height);
                    img.Dispose();

                    string fileExt = Path.GetExtension(artwork);
                    if (string.IsNullOrEmpty(thumbnail))
                    {
                        thumbnail = artwork.Replace(fileExt, string.Format("_{0}-{1}{2}", width, height, fileExt));
                    }
                    thumImg.Save(localPath.Replace(fileExt, string.Format("_{0}-{1}{2}", width, height, fileExt)), getFormat(fileExt));
                    thumImg.Dispose();

                    return thumbnail; //返回缩略图地址
                }

            }
            else//原图不存在
            {
                return null;//返回空
            }

        }
        #endregion;

        #region 水印处理
        /// <summary>
        /// 水印处理
        /// </summary>
        public static void setWatermark(Image img, Entity.UploadConfigInfo config)
        {
            if (img.Width > 399 && img.Height > 399)//过小图片不加水印
            {
                switch (config.WatermarkType)//水印
                {
                    case 1:
                        watermarkText(img, config);
                        break;
                    case 2:
                        watermarkPic(img, config);
                        break;
                }
            }
        }

        #region 文字水印
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        private static void watermarkText(Image upImg, Entity.UploadConfigInfo config)
        {
            Graphics g = Graphics.FromImage(upImg);
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();

            var size = upImg.Width / config.WatermarkText.Length / 5;
            if (size < 4)
            {
                size = 4;
            }

            crFont = new Font(config.WatermarkFont, size, FontStyle.Bold);
            crSize = g.MeasureString(config.WatermarkText, crFont);

            float xpos = 0;
            float ypos = 0;

            switch (config.WatermarkStatus)// 0=左上 1=中上 2=右上 3=左中 ... 8=右下
            {
                case 0:
                    xpos = 10 + (float)(crSize.Width / 2);
                    ypos = 10;
                    break;
                case 1:
                    xpos = (float)upImg.Width * (float).49;
                    ypos = 10;
                    break;
                case 2:
                    xpos = (float)upImg.Width - (crSize.Width / 2) - 10;
                    ypos = 10;
                    break;
                case 3:
                    xpos = 10 + (float)(crSize.Width / 2);
                    ypos = (float)upImg.Height * (float).49;
                    break;
                case 4:
                    xpos = (float)upImg.Width * (float).49;
                    ypos = (float)upImg.Height * (float).49;
                    break;
                case 5:
                    xpos = ((float)upImg.Width) - (crSize.Width / 2) - 10;
                    ypos = (float)upImg.Height * (float).49;
                    break;
                case 6:
                    xpos = 10 + (float)(crSize.Width / 2);
                    ypos = (float)upImg.Height - crSize.Height - 10;
                    break;
                case 7:
                    xpos = (float)upImg.Width * (float).49;
                    ypos = (float)upImg.Height - crSize.Height - 10;
                    break;
                default:
                    xpos = (float)upImg.Width - (crSize.Width / 2) - 10;
                    ypos = (float)upImg.Height - crSize.Height - 10;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            g.DrawString(config.WatermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            g.DrawString(config.WatermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);

            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();

            g.Dispose();
        }
        #endregion

        #region 图片水印
        ///<summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        private static void watermarkPic(Image upImg, Entity.UploadConfigInfo config)
        {
            Graphics g = Graphics.FromImage(upImg);
            var watermarkpic = config.WatermarkPic;
            if (string.IsNullOrEmpty(watermarkpic))
            {
                watermarkpic = "watermark.png";
            }
            if (!watermarkpic.StartsWith("/"))
            {
                watermarkpic = "/images/" + watermarkpic;
            }

            var filepath = bitcms.Common.Fetch.getMapPath(watermarkpic);
            if (!File.Exists(filepath))
            {
                return;
            }
            Image watermark = Image.FromFile(filepath);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            var transparency = config.WatermarkTransparency / 10f;

            float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                             };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            int watermarkWidth = 0;
            int watermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((upImg.Width > watermark.Width * 4) && (upImg.Height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((upImg.Width > watermark.Width * 4) && (upImg.Height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(upImg.Height / 4) / Convert.ToDouble(watermark.Height);
            }
            else

                if ((upImg.Width < watermark.Width * 4) && (upImg.Height > watermark.Height * 4))
                {
                    bl = Convert.ToDouble(upImg.Width / 4) / Convert.ToDouble(watermark.Width);
                }
                else
                {
                    if ((upImg.Width * watermark.Height) > (upImg.Height * watermark.Width))
                    {
                        bl = Convert.ToDouble(upImg.Height / 4) / Convert.ToDouble(watermark.Height);
                    }
                    else
                    {
                        bl = Convert.ToDouble(upImg.Width / 4) / Convert.ToDouble(watermark.Width);
                    }
                }

            watermarkWidth = Convert.ToInt32(watermark.Width * bl);
            watermarkHeight = Convert.ToInt32(watermark.Height * bl);

            switch (config.WatermarkStatus)
            {
                case 0:
                    xpos = 10;
                    ypos = 10;
                    break;
                case 1:
                    xpos = (upImg.Width - watermarkWidth) / 2;
                    ypos = 10;
                    break;
                case 2:
                    xpos = upImg.Width - watermarkWidth - 10;
                    ypos = 10;
                    break;
                case 3:
                    xpos = 10;
                    ypos = (upImg.Height - watermarkHeight) / 2;
                    break;
                case 4:
                    xpos = (upImg.Width - watermarkWidth) / 2;
                    ypos = (upImg.Height - watermarkHeight) / 2;
                    break;
                case 5:
                    xpos = upImg.Width - watermarkWidth - 10;
                    ypos = (upImg.Height - watermarkHeight) / 2;
                    break;
                case 6:
                    xpos = 10;
                    ypos = upImg.Height - watermarkHeight - 10;
                    break;
                case 7:
                    xpos = (upImg.Width - watermarkWidth) / 2;
                    ypos = upImg.Height - watermarkHeight - 10;
                    break;
                default:
                    xpos = upImg.Width - watermarkWidth - 10;
                    ypos = upImg.Height - watermarkHeight - 10;
                    break;

            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermarkWidth, watermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            g.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }
        #endregion
        #endregion

        #region 裁剪并缩放
        /// <summary>
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="bigImg">原图Bitmap对象</param>
        private static Image getAttachThumbnail(Bitmap bigImg, int width, int height)
        {

            //模版的宽高比例
            double templateRate = (double)width / height;
            //原图片的宽高比例
            double initRate = (double)bigImg.Width / bigImg.Height;

            //原图与模版比例相等，直接缩放
            if (templateRate == initRate)
            {
                //return bigImg.GetThumbnailImage(width, height, null, IntPtr.Zero);
                //按模版大小生成最终图片
                return new Bitmap(bigImg, resizeImage(bigImg.Width, bigImg.Height, width, height));//获取缩略图
            }
            //原图与模版比例不等，裁剪后缩放
            else
            {
                //缩略图
                var bitmap = new Bitmap(bigImg, resizeImage(bigImg.Width, bigImg.Height, width, height, false));//获取缩略图
                //定位
                Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                //宽为标准进行裁剪
                if (templateRate > initRate)
                {
                    //裁剪源定位
                    fromR.X = 0;
                    fromR.Y = (int)Math.Floor((bitmap.Height - bitmap.Width / templateRate) / 2);
                    fromR.Width = bigImg.Width;
                    fromR.Height = (int)Math.Floor(bitmap.Width / templateRate);

                    //裁剪目标定位
                    toR.X = 0;
                    toR.Y = 0;
                    toR.Width = bigImg.Width;
                    toR.Height = (int)Math.Floor(bitmap.Width / templateRate);
                }
                //高为标准进行裁剪
                else
                {
                    fromR.X = (int)Math.Floor((bitmap.Width - bitmap.Height * templateRate) / 2);
                    fromR.Y = 0;
                    fromR.Width = (int)Math.Floor(bitmap.Height * templateRate);
                    fromR.Height = bigImg.Height;

                    toR.X = 0;
                    toR.Y = 0;
                    toR.Width = (int)Math.Floor(bitmap.Height * templateRate);
                    toR.Height = bigImg.Height;
                }

                //裁切图
                Bitmap thum = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(thum);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(bitmap, toR, fromR, GraphicsUnit.Pixel);

                bitmap.Dispose();
                g.Dispose();

                return thum;
            }
        }
        #endregion

        #region 等比缩放 获取新尺寸
        /// <summary>
        /// 计算新尺寸
        /// </summary>
        /// <param name="Width">原始宽度</param>
        /// <param name="Height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
        /// <param name="smallset"></param>
        /// <returns></returns>
        private static Size resizeImage(int width, int height, int maxWidth, int maxHeight, bool smallset = true)
        {
            decimal max_width = (decimal)maxWidth;
            decimal max_height = (decimal)maxHeight;
            decimal ratio = max_width / max_height;

            int newWidth, newHeight;
            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (originalWidth > max_width || originalHeight > max_height)
            {
                decimal factor;

                // determine the largest factor 
                if (originalWidth / originalHeight > ratio)
                {
                    if (smallset)
                    {
                        factor = originalWidth / max_width;
                    }
                    else
                    {
                        factor = originalHeight / max_height;
                    }
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    if (smallset)
                    {
                        factor = originalHeight / max_height;
                    }
                    else
                    {
                        factor = originalWidth / max_width;
                    }
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            return new Size(newWidth, newHeight);
        }
        #endregion

        #region 获取图片格式
        /// <summary>
        /// 获取图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        private static ImageFormat getFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        #endregion
    }
}
