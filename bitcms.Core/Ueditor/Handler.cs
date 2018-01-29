using System;
using System.Web;
using Newtonsoft.Json;

namespace bitcms.Ueditor
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public abstract class Handler
    {
        public Handler(HttpContextBase context)
        {
            this.Request = context.Request;
            this.Response = context.Response;
            this.Context = context;
            this.Server = context.Server;
        }

        public abstract string Process();

        protected string WriteJson(object response)
        {
            string jsonpCallback = Request["callback"],
                json = JsonConvert.SerializeObject(response);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                return json;
            }
            else
            {
                return String.Format("{0}({1});", jsonpCallback, json);
            }
        }

        public HttpRequestBase Request { get; private set; }
        public HttpResponseBase Response { get; private set; }
        public HttpContextBase Context { get; private set; }
        public HttpServerUtilityBase Server { get; private set; }
    }
}