using System.Web;
namespace bitcms.Ueditor
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContextBase context) : base(context) { }

        public override string Process()
        {
           return WriteJson(Config.Items);
        }
    }
}