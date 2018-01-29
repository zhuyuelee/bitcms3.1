using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
namespace bitcms.Ueditor
{
    /// <summary>
    /// Crawler 的摘要说明
    /// </summary>
    public class CrawlerHandler : Handler
    {
        private string[] Sources;
        private Crawler[] Crawlers;
        public CrawlerHandler(HttpContextBase context) : base(context) { }

        public override string Process()
        {
            Sources = Request.Form.GetValues("source[]");
            if (Sources == null || Sources.Length == 0)
            {
                return WriteJson(new
                  {
                      state = "参数错误：没有指定抓取源"
                  });
            }
            Crawlers = Sources.Select(x => new Crawler(x, Server).Fetch()).ToArray();
            return WriteJson(new
             {
                 state = "SUCCESS",
                 list = Crawlers.Select(x => new
                 {
                     state = x.State,
                     source = x.SourceUrl,
                     url = x.ServerUrl
                 })
             });
        }
    }

    public class Crawler
    {
        public string SourceUrl { get; set; }
        public string ServerUrl { get; set; }
        public string State { get; set; }

        private HttpServerUtilityBase Server { get; set; }


        public Crawler(string sourceUrl, HttpServerUtilityBase server)
        {
            this.SourceUrl = sourceUrl;
            this.Server = server;
        }

        public Crawler Fetch()
        {
            if (!IsExternalIPAddress(this.SourceUrl))
            {
                State = "INVALID_URL";
                return this;
            }


            var request = HttpWebRequest.Create(this.SourceUrl);
            request.Method = "GET";
            using (var response = request.GetResponse())
            {
                //if (response.StatusCode != HttpStatusCode.OK)
                //{
                //    State = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                //    return this;
                //}
                if (response.ContentType.IndexOf("image") == -1)
                {
                    State = "Url is not an image";
                    return this;
                }
                var sourceUrl = Path.GetFileName(this.SourceUrl);
                string fileExt = Path.GetExtension(sourceUrl).ToLower();//扩展名
                if (string.IsNullOrEmpty(fileExt))
                {
                    var strs = response.ContentType.ToLower().Split('/');
                    if(strs.Length==2)
                    {
                        fileExt = "." + strs[1];
                    }
                }

                var upload = new Common.Upload();
                string filePath = upload.getFilePath("catcher");
                string fileName = upload.getFileName(fileExt);
                try
                {
                    var stream = response.GetResponseStream();
                    if (fileExt.Equals(".gif"))//gif文件
                    {
                        using (FileStream writer = new FileStream(Common.Fetch.getMapPath(filePath + fileName), FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            byte[] buff = new byte[512];
                            int c = 0; //实际读取的字节数 
                            while ((c = stream.Read(buff, 0, buff.Length)) > 0)
                            {
                                writer.Write(buff, 0, c);
                            }
                            ServerUrl = filePath + fileName;
                            State = "SUCCESS";
                        }
                    }
                    else
                    {
                       
                        ServerUrl = upload.saveImage(Image.FromStream(stream), fileName, filePath);
                        State = "SUCCESS";
                    }
                    stream.Dispose();
                }
                catch (Exception e)
                {
                    State = "抓取错误：" + e.Message;
                }
                return this;
            }
        }

        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntry(uri.DnsSafeHost);
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }

        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }
    }
}