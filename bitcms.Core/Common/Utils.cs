using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace bitcms.Common
{
    public class Utils
    {
        #region 正则
        /// <summary>
        /// 验证与指定正则表达式字符相匹配
        /// </summary>
        /// <param name="reg">正则表达式字符串</param>
        /// <param name="str">要进行匹配的原字符串</param>
        /// <returns>返回与表达式相匹配的结果</returns>
        public static bool verifyMatch(string reg, string str)
        {
            Regex r = new Regex(reg, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return r.IsMatch(str);
        }

        /// <summary>
        /// 获取与指定正则表达式字符相匹配的结果
        /// </summary>
        /// <param name="reg">正则表达式字符串</param>
        /// <param name="sourceStr">要进行匹配的原字符串</param>
        /// <returns>返回与表达式相匹配的结果</returns>
        public static Match getMatch(string reg, string sourceStr)
        {
            Regex r = new Regex(reg, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return r.Match(sourceStr);
        }

        /// <summary>
        /// 获取匹配的正则表达式的值
        /// </summary>
        /// <param name="reg">要匹配的正则表达式</param>
        /// <param name="sourceS">要进行匹配的原字符串</param>
        /// <returns>返回相匹配的字符串</returns>
        public static string getMatchValue(string reg, string str)
        {
            string result = string.Empty;

            Match match = getMatch(reg, str);

            if (match.Success)
            {
                result = match.Value;
            }

            return result;
        }

        /// <summary>
        /// 获取相匹配的正则表达式结果的集合
        /// </summary>
        /// <param name="reg">正则表达式</param>
        /// <param name="str">要进行匹配的字符串</param>
        /// <returns>返回相匹配的正则表达式结果的集合</returns>
        public static MatchCollection getMatches(string reg, string str)
        {
            Regex r = new Regex(reg, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return r.Matches(str);
        }
        #endregion

        #region 验证
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool isIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="email">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool isEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        /// <summary>
        /// 检测是否符合手机格式
        /// </summary>
        /// <param name="strEmail">要判断的手机字符串</param>
        /// <returns>判断结果</returns>
        public static bool isMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^1[3|4|5|6|7|8|9][0-9]\d{8}$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="url">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool isURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 是否为有效域
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public static bool isDomain(string domain)
        {
            if (domain.IndexOf(".") == -1)
                return false;

            return new Regex(@"^\d+$").IsMatch(domain.Replace(".", string.Empty)) ? false : true;
        }

        /// <summary>
        /// 判断字符串是否是yy-mm-dd字符串
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool isDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool inIPArray(string ip, string[] iparray)
        {
            string[] userip = Utils.splitString(ip, @".");

            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = Utils.splitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                        return true;

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                            r++;
                        else
                            break;
                    }
                    else
                        break;
                }

                if (r == 4)
                    return true;
            }
            return false;
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string bitcmsMD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            StringBuilder sb_ret = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
                sb_ret.Append((b[i] >> 1).ToString("x").PadLeft(2, '0'));

            return sb_ret.ToString();
        }
        #endregion

        #region 随机数
        /// </summary>
        /// 随机产生字符串
        /// </summary>
        /// <returns></returns>
        public static string random(int len)
        {
            return random(len, true);
        }

        /// <summary>
        /// 随机产生字符串
        /// </summary>
        /// <returns></returns>
        public static string random(int len, bool number)
        {
            string code = "";
            ArrayList source = new ArrayList();
            string[] num = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            source.AddRange(num);
            if (!number)
            {
                string[] ABC =  {"a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
                                 "A","B","C","D","E","F","G","H","J","K","M","N","P","Q","R","S","T","U","V","W","X","Y","Z"};
                source.AddRange(ABC);
            }
            Random rd = new Random();
            for (int i = 0; i < len; i++)
            {
                code += source[rd.Next(0, source.Count)];
            }
            return code;
        }

        #endregion

        #region 字符处理
        /// <summary>  
        /// 获取Unix时间戳格式  
        /// </summary>  
        /// <returns>Unix时间戳格式</returns>  
        public static Int64 getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (Int64)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string htmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string htmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string urlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string urlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 正则去除字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string remove(string str, string pattern)
        {
            return replace(str, pattern, "");
        }

        /// <summary>
        /// 正则替换字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <param name="newchar"></param>
        /// <returns></returns>
        public static string replace(string str, string pattern, string newchar)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Regex.Replace(str, pattern, newchar);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///去除前后指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimstr"></param>
        /// <returns></returns>
        public static string trim(string str, string trimstr)
        {
            str = trimStart(str, trimstr);
            str = trimEnd(str, trimstr);

            return str;
        }
        /// <summary>
        /// 去除前面指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimstr"></param>
        /// <returns></returns>
        public static string trimStart(string str, string trimstr)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (str.StartsWith(trimstr))
            {
                string startPat = string.Format("^({0})*", trimstr);
                str = Regex.Replace(str, startPat, "", RegexOptions.Singleline);//头部
            }
            return str;
        }

        /// <summary>
        /// 去除后面指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimstr"></param>
        /// <returns></returns>
        public static string trimEnd(string str, string trimstr)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (str.EndsWith(trimstr))
            {
                string startPat = string.Format("({0})*$", trimstr);
                str = Regex.Replace(str, startPat, "", RegexOptions.Singleline);//头部
            }
            return str;
        }

        /// <summary>
        /// 过滤Sql危险字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string removeUnSafeString(string str)
        {
            string strReg = "[-|\\/|\\(|\\)|\\[|\\]|\\}|\\{|%|@|?|.|#|$|*|~|^|`|\\*|!|\\'|\"]";
            Regex regex = new Regex(strReg);

            return regex.Replace(str, new MatchEvaluator(toSBC));
        }

        /// <summary>
        /// 转全角字符
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string toSBC(Match match)
        {
            string str = match.Value;
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            // 半角转全角：
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == 32)
                {
                    chars[i] = (char)12288;
                    continue;
                }
                if (chars[i] < 127)
                    chars[i] = (char)(chars[i] + 65248);
            }
            return new String(chars);
        }
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string removeUnsafeHtml(string content)
        {
            if (string.IsNullOrEmpty(content))
                return "";
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            content = regex3.Replace(content, " _disibledevent="); //过滤其它控件的on...事件
            return content;
        }

        /// <summary>
        /// 去除A标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string removeLink(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = Regex.Replace(str, @"<a[^>]*?>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"</a>", "", RegexOptions.IgnoreCase);

            return str;
        }

        /// <summary>
        /// 过滤字符串中的html代码
        /// </summary>
        /// <param name="Str"></param>
        /// <returns>返回过滤之后的字符串</returns>
        public static string removeHTML(string str)
        {
            string re_Str = "";
            if (!string.IsNullOrEmpty(str))
            {
                string Pattern = "<\\/*[^<>]*>";
                re_Str = Regex.Replace(str, Pattern, "");
            }
            return (re_Str.Replace("\\r\\n", "")).Replace("\\r", "").Replace("&nbsp;", "").Replace("\\s+", "");
        }


        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] splitString(string str, string pattern)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.IndexOf(pattern) < 0)
                    return new string[] { str };

                return Regex.Split(str, Regex.Escape(pattern), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <param name="length">指定长度</param>
        /// <param name="pattern">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string subString(string str, int length)
        {
            return subString(str, 0, length, null);
        }
        /// <summary>
        /// 截取指定长度字符串,超出的部分用指定字符串代替
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <param name="length">指定长度</param>
        /// <param name="pattern">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string subString(string str, int length, string pattern)
        {
            return subString(str, 0, length, pattern);
        }

        /// <summary>
        /// 截取指定开始位置和指定长度字符串,超出的部分用指定字符串代替
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <param name="startindex">起始位置</param>
        /// <param name="length">指定长度</param>
        /// <param name="pattern">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string subString(string str, int startindex, int length, string pattern)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            string result = str;

            if (length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(str);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > startindex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (startindex + length))
                    {
                        p_EndIndex = length + startindex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾
                        length = bsSrcString.Length - startindex;
                        pattern = "";
                    }

                    int nRealLength = length;
                    int[] anResultFlag = new int[length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = startindex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[length - 1] == 1))
                        nRealLength = length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, startindex, bsResult, 0, nRealLength);

                    result = Encoding.Default.GetString(bsResult);
                    if (!string.IsNullOrEmpty(pattern))
                    {
                        result += pattern;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 替换字符
        /// </summary>
        public static string fuzzyString(string str, int startIndx)
        {
            return fuzzyString(str, startIndx, str.Length - startIndx, '*');
        }
        /// <summary>
        /// 从指定位置替换字符
        /// </summary>
        public static string fuzzyString(string str, int startIndx, int length)
        {
            return fuzzyString(str, startIndx, length, '*');
        }
        /// <summary>
        /// 从指定位置，长度替换字符
        /// </summary>
        public static string fuzzyString(string str, int startIndx, int length, char pattern)
        {
            if (str.Length > startIndx + length)
            {
                string code = "";
                for (int i = 0; i < length; i++)
                {
                    code += pattern;
                }
                return str.Substring(0, startIndx - 1) + code + str.Substring(startIndx - 1 + length);
            }
            else if (str.Length > startIndx)
            {
                string code = "";
                for (int i = 0; i < str.Length - startIndx + 1; i++)
                {
                    code += pattern;
                }
                return str.Substring(0, startIndx) + code;
            }
            else
            {
                return str;
            }

        }


        /// <summary>
        /// 返回时间差
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static string dateDiff(DateTime dt1, DateTime dt2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts = dt2 - dt1;
                if (ts.Days >= 365)
                {
                    return dt2.ToString("yyyy年MM月dd日");
                }
                else if (ts.Days >= 30)
                {
                    dateDiff = ts.Days / 30 + "月前";
                }
                else if (ts.Days >= 7)
                {
                    dateDiff = ts.Days / 7 + "周前";
                }
                else if (ts.Days >= 1)
                {
                    dateDiff = ts.Days + "天前";
                }
                else
                {
                    if (ts.Hours >= 1)
                    {
                        dateDiff = ts.Hours + "小时前";
                    }
                    else if (ts.Milliseconds > 2)
                    {
                        dateDiff = ts.Minutes + "分钟前";
                    }
                    else
                    {
                        return "刚刚";
                    }
                }
            }
            catch
            {
                dateDiff = "";
            }
            return dateDiff;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 将字符转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int strToInt(string str)
        {
            return strToInt(str, 0);
        }

        /// <summary>
        /// 将字符转换为Int32类型,转换失败返回defaultvalue
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultvalue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int strToInt(string str, int defaultvalue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                Int32.TryParse(str, out defaultvalue);
            }
            return defaultvalue;
        }

        /// <summary>
        /// 将字符转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime strToDateTime(string str)
        {
            return strToDateTime(str, DateTime.MinValue);
        }


        /// <summary>
        /// 将字符转换为日期时间类型,转换失败返回defaultvalue
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defaultvalue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime strToDateTime(string str, DateTime defaultvalue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime.TryParse(str, out defaultvalue);
            }
            return defaultvalue;
        }

        /// <summary>
        /// 将字符转换为Decimal型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Decimal strToDecimal(string str)
        {
            return strToDecimal(str, 0);
        }

        /// <summary>
        /// 将字符转换为Decimal型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultvalue">缺省值</param>
        /// <returns>转换后的Decimal类型结果</returns>
        public static Decimal strToDecimal(string str, Decimal defaultvalue)
        {

            if (!string.IsNullOrEmpty(str))
            {
                if (Regex.IsMatch(str, @"^(0|[1-9]\d{0,15})(\.\d{0,5})?$"))
                    Decimal.TryParse(str, out defaultvalue);
            }
            return defaultvalue;
        }
        #endregion

        #region 分页处理

        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="pageindex">当前页数</param>
        /// <param name="countPage">页面总数</param>
        /// <param name="pagename">页面地址</param>
        /// <param name="extend">周边页码显示个数上限</param>
        /// <param name="tag">分页标签</param>
        /// <returns>页码html</returns>
        public static string getPages(int pageindex, int pagecount, string pagename, int extend, string tag = null)
        {
            int startPage = 1;
            int endPage = 1;

            if (pagename.IndexOf("{0}") == -1)
            {
                if (tag == "")
                {
                    tag = "page";
                }

                if (pagename.IndexOf("?") > 0)
                    pagename = pagename + "&" + tag + "={0}";
                else
                    pagename = pagename + "?" + tag + "={0}";
            }

            string pageName1 = pagename;

            if (string.IsNullOrEmpty(tag))//重写或静态
            {
                pageName1 = Regex.Replace(pageName1, @"[-_]{1}\{0\}", "", RegexOptions.IgnoreCase);
            }
            else//URL分页
            {
                if (pagename.IndexOf("&" + tag) > -1)
                    pageName1 = Regex.Replace(pageName1, @"\&" + tag + @"=\{0\}", "", RegexOptions.IgnoreCase);
                else if (pagename.IndexOf("?" + tag) > -1)
                    pageName1 = Regex.Replace(pageName1, @"\?" + tag + @"=\{0\}", "", RegexOptions.IgnoreCase);
            }

            string p = "<a href=\"" + pagename + "\">{0}</a>&nbsp;";
            string p1 = "<a href=\"" + pageName1 + "\">{0}</a>&nbsp;";
            string curP = "<span>{0}</span>&nbsp;";


            if (pageindex < 1)
                pageindex = 1;
           
            if (pageindex > pagecount)
                pageindex = pagecount;

            if (pagecount > extend)
            {
                if (pageindex - (extend / 2) > 0)
                {
                    if (pageindex + (extend / 2) < pagecount)
                    {
                        startPage = pageindex - (extend / 2);
                        endPage = startPage + extend - 1;
                    }
                    else
                    {
                        endPage = pagecount;
                        startPage = endPage - extend + 1;
                    }
                }
                else
                {
                    endPage = extend;
                }
            }
            else
            {
                startPage = 1;
                endPage = pagecount;
            }

            StringBuilder sb_page = new StringBuilder();


            if (pageindex == 2)//显示上一页
            {
                sb_page.Append(string.Format("<a class=\"pre_page\" href=\"" + pageName1 + "\">{1}</a>&nbsp;", pageindex - 1, "上一页"));
            }
            else if (pageindex > 2)
            {
                sb_page.Append(string.Format("<a class=\"pre_page\" href=\"" + pagename + "\">{1}</a>&nbsp;", pageindex - 1, "上一页"));
            }
            if (extend > 0)
            {
                if (startPage > 2)//显示第一页
                {
                    sb_page.Append(string.Format(p1, 1));
                    sb_page.Append("...&nbsp;");
                }
                else if (startPage == 2)
                {
                    sb_page.Append(string.Format(p1, 1));
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    if (i == pageindex)
                    {
                        sb_page.Append(string.Format(curP, i));
                    }
                    else
                    {
                        if (i == 1)//第一页
                        {
                            sb_page.Append(string.Format(p1, i));
                        }
                        else
                        {
                            sb_page.Append(string.Format(p, i));
                        }
                    }
                }
                if (pagecount - endPage >= 2)
                {
                    sb_page.Append("...&nbsp;");
                }
                if (endPage < pagecount)
                {
                    sb_page.Append(string.Format(p, pagecount));
                }
            }
            if (pagecount > 1 && pageindex < pagecount)
            {
                sb_page.Append(string.Format("<a class=\"next_page\" href=\"" + pagename + "\">{1}</a>&nbsp;", pageindex + 1, "下一页"));
            }

            return sb_page.ToString();
        }
        #endregion
    }
}
