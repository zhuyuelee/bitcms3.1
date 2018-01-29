using System;
using System.IO;
using System.Xml.Serialization;

namespace bitcms.Common
{
    /// <summary>    
    /// SerializationHelper 的摘要说明。    
    /// </summary>   
    public class SerializationHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数        
        /// </summary>        
        private SerializationHelper() { }
        #endregion

        #region 反序列化
        /// <summary>        
        /// 反序列化        
        /// </summary>        
        /// <param name="Type">对象类型</param>        
        /// <param name="fileName">文件路径</param>        
        /// <returns></returns>        
        public static object load(Type type, string fileName)
        {
            return load(type, fileName, null);
        }

        /// <summary>        
        /// 反序列化        
        /// </summary>        
        /// <param name="Type">对象类型</param>        
        /// <param name="fileName">文件路径</param>        
        /// <param name="root">根元素</param>        
        /// <returns></returns>       
        public static object load(Type type, string fileName, string root)
        {
            FileStream fs = null;
            //System.Collections.Specialized.StringDictionary dictionary = new System.Collections.Specialized.StringDictionary();
            string cacheKey = Utils.MD5("bitcms_Xml_" + root + "_" + fileName);
            object obj = DataCache.get(cacheKey);
            if (obj == null)
            {
                try
                {
                    // 打开 FileStream                
                    using (fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        XmlSerializer serializer;
                        if (root != null)
                        {
                            XmlRootAttribute xmlRoot = new XmlRootAttribute();
                            xmlRoot.ElementName = root;
                            serializer = new XmlSerializer(type, xmlRoot);
                        }
                        else
                        {
                            serializer = new XmlSerializer(type);
                        }
                        obj = serializer.Deserialize(fs);
                        //建立缓存依赖项
                        System.Web.Caching.CacheDependency cdd = new System.Web.Caching.CacheDependency(fileName);
                        DataCache.set(cacheKey, obj, cdd, DateTime.Now.AddDays(1));
                    }
                }
                catch (Exception e)
                {
                    Logs.error("SerializationHelper 反序列化", fileName + " " + e.Message);
                    obj = null;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }

            return obj;
        }
        #endregion

        #region 序列化
        /// <summary>        
        /// 序列化        
        /// </summary>        
        /// <param name="obj">对象</param>       
        /// <param name="fileName">文件路径</param>       
        public static bool save(object obj, string fileName)
        {
            return save(obj, fileName, null);
        }

        /// <summary>       
        /// 序列化      
        /// </summary>        
        /// <param name="obj">对象</param>       
        /// <param name="fileName">文件路径</param>       
        /// <param name="Root">根元素</param>       
        /// <returns></returns>        
        public static bool save(object obj, string fileName, string root)
        {
            bool reResult = true;
            FileStream fs = null;
            try
            {
                using (fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    XmlSerializer serializer;
                    if (root != null)
                    {
                        XmlRootAttribute xmlRoot = new XmlRootAttribute();
                        xmlRoot.ElementName = root;
                        serializer = new XmlSerializer(obj.GetType(), xmlRoot);
                    }
                    else
                    {
                        serializer = new XmlSerializer(obj.GetType());
                    }
                    serializer.Serialize(fs, obj);
                }

            }
            catch (Exception e)
            {
                Logs.error("SerializationHelper 序列化", fileName + " " + e.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return reResult;
        }


        #endregion
    }
}
