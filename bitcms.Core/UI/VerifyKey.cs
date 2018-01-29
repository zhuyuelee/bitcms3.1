using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using bitcms.Common;

namespace bitcms.UI
{
    /// <summary>
    /// 验证码生成类
    /// </summary>
    public class VerifyKey
    {
        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Image getVerifyKey(string code, int width = 0, int height = 0)
        {
            //字体列表，用于验证码 
            string[] font = { "Comic Sans MS", "Times New Roman", "MS Mincho", "Book Antiqua", 
                "Gungsuh", "Arial" };
            //
            if (width < 60)
                width = code.Length * 12;
            if (height < 30)
                height = 30;
            var image = new Bitmap(width, height);

            var config = Config.SiteConfig.load();

            var bg = config.VerifykeyBackGround;
            if (string.IsNullOrEmpty(bg))
            {
                bg = "verifykeybg.jpg";
            }
            if (!bg.StartsWith("/"))
            {
                bg = "/images/" + bg;
            }
            var filepath = bitcms.Common.Fetch.getMapPath(bg);
            if (File.Exists(filepath))
            {
                var bgimage = new Bitmap(filepath);
                Random rd = new Random();
                var x = rd.Next(0, bgimage.Width - width);
                var y = rd.Next(0, bgimage.Height - height);

                image = bgimage.Clone(new System.Drawing.Rectangle(x, y, width, height), bgimage.PixelFormat);
                bgimage.Dispose();
            }
            var g = Graphics.FromImage(image);


            Random rnd = new Random();
            var codeWidth = (float)width / code.Length;

            //画验证码字符串 
            for (int i = 0; i < code.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                var size = rnd.Next(12, 15);
                Font ft = new Font(fnt, size);
                string c = code[i].ToString();
                var x = (float)i * codeWidth + rnd.Next(0 - size / 4, size / 4);
                if (i * 2 > code.Length)
                {
                    x -= size / 4;
                }
                else
                {
                    x += size / 4;
                }

                if (rnd.Next(0, 1) == 0)
                    c = c.ToLower();//随机大小写
                g.DrawString(c,
                ft, new SolidBrush(Color.FromArgb(200, Color.White)), x, rnd.Next(0, height / 2));
            }
            return image;
        }
    }
}
