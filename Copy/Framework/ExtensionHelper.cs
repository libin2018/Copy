using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Copy
{
    public static class ExtensionHelper
    {
        public static string GetJValue(this JObject o, string key)
        {
            return o.GetValue(key, StringComparison.CurrentCultureIgnoreCase).ToString();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T entity)
        {
            return entity == null ? string.Empty : JsonConvert.SerializeObject(entity);
        }

        public static double? TimeDifference(this DateTime fromDateTime, DateTime? currDateTime)
        {
            if (currDateTime.HasValue)
            {
                TimeSpan fts = new TimeSpan(fromDateTime.Ticks);
                TimeSpan cts = new TimeSpan(currDateTime.Value.Ticks);
                TimeSpan ts = fts.Subtract(cts);
                return ts.TotalSeconds;
            }
            return null;
        }

        /// <summary>
        /// 去掉字符串所有的空格,指定截取长度
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="length">指定截取长度,空的话表示知识去掉空格</param>
        /// <returns></returns>
        public static string TrimSubstring(this string source, int? length = null)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            if (!length.HasValue)
            {
                return source.Replace(" ", null).Trim();
            }
            result = source.Replace(" ", null).Trim();
            if (result.Length > length.Value)
            {
                result = result.Substring(0, length.Value);
            }
            return result;
        }

        /// <summary>
        /// 判定string 类型为 空 和0
        /// </summary>
        /// <param name="source"></param>
        /// <param name="opposite">相反,默认是true,如要取反结果则为true</param>
        /// <returns></returns>
        public static bool IsNullOrZore(this string source, bool opposite = true)
        {
            var result = string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source) || source.Trim() == "0";
            return opposite ? result : !result;
        }

        /// <summary>
        /// 比较是否存在对应数据,去空格，忽略大小写,是否包含字符串
        /// </summary>
        /// <param name="paras">待判定数据</param>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static bool InStr(this string paras, params string[] source)
        {
            if (string.IsNullOrEmpty(paras) || source == null || source.Length == 0)
            {
                return false;
            }
            foreach (var item in source)
            {
                if (!string.IsNullOrEmpty(item) && string.Compare(paras.Trim(), item.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 去空格 NULL转string.empty
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static string ToTrim(this string paras)
        {
            return string.IsNullOrEmpty(paras) ? string.Empty : paras.Trim();
        }

        /// <summary>
        /// Contains比较是否存在对应数据,忽略大小写
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Like(this string paras, string key)
        {
            if (string.IsNullOrEmpty(paras))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            else
            {
                return paras.ToLower().Contains(key.ToLower());
            }
        }

        /// <summary>
        /// 是否当月
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static bool IsCurrMonth(this DateTime paras)
        {
            return paras.Year == DateTime.Now.Year && paras.Month == DateTime.Now.Month;
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="key"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToTimeStr(this DateTime? key, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return key.HasValue ? key.Value.ToString(format) : "";
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="key"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateStr(this DateTime? key, string format = "yyyy-MM-dd")
        {
            return key.HasValue ? key.Value.ToString(format) : "";
        }

        /// <summary>
        /// 等同StartsWith 排除空格及大小写
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool StartsWithStr(this string paras, string source)
        {
            if (string.IsNullOrWhiteSpace(paras) || string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return paras.Trim().StartsWith(source.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 字符串扩展
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetValueOrDefault(this string str, string defVal)
        {
            return string.IsNullOrEmpty(str) ? defVal : str;
        }

        /// <summary>
        /// 整形处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool InArray<T>(this T paras, params T[] source)
        {
            if (source == null || source.Length == 0)
            {
                return false;
            }
            return source.Contains(paras);
        }

        /// <summary>
        /// 逻辑位移包含 1 2 4 8，0为包含所有
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool InAND(this int source, int key)
        {
            return source == 0 || (source & key) == key;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string paras) where T : class
        {
            if (IsNullOrZore(paras))
            {
                return null;
            }
            if (typeof(T).Name == "String" || typeof(T).IsValueType)
            {
                return paras as T;
            }
            return JsonConvert.DeserializeObject<T>(paras);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(this T paras, string dataFormat = "yyyy-MM-dd hh:mm:ss") where T : class
        {
            if (paras == null)
            {
                return null;
            }
            if (typeof(T).IsValueType)
            {
                return paras.ToString();
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = dataFormat;
            return JsonConvert.SerializeObject(paras);
        }

        /// <summary>
        /// XML格式字符串发序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(this string paras) where T : class
        {
            if (IsNullOrZore(paras))
            {
                return null;
            }
            T cloneObject = default(T);
            StringBuilder buffer = new StringBuilder();
            buffer.Append(paras);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(buffer.ToString()))
            {
                Object obj = serializer.Deserialize(reader);
                cloneObject = (T)obj;
            }
            return cloneObject;
        }

        /// <summary>
        /// 将对象序列化为xml格式字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(this T paras)
        {
            if (paras == null)
            {
                return null;
            }
            using (var sw = new StringUTF8Writer())
            {
                XmlSerializer xz = new XmlSerializer(paras.GetType());
                xz.Serialize(sw, paras);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 多个key,对应同一value值,一次性添加
        /// </summary>
        /// <typeparam name="TK">key类型</typeparam>
        /// <param name="paras">字典</param>
        /// <param name="val">value值</param>
        /// <param name="keys">key值</param>
        public static void MultipleKeySingleVal<TK>(this System.Collections.Generic.Dictionary<string, TK> paras, TK val, params string[] keys)
        {
            if (keys == null) return;
            foreach (var item in keys)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    paras.Add(item.Trim().ToLower(), val);
                }
            }
        }

        /// <summary>
        /// if判定验证,返回消息(主要是为了缓解代码行数)
        /// </summary>
        /// <param name="isTrue"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string PackageMsg(this bool isTrue, string msg)
        {
            if (isTrue)
            {
                return msg;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIP(this HttpRequestMessage request)
        {
            return ComHelper.GetIP(request);
        }

        #region 返回值处理

        public static APIActionResult<T> Opt<T>(this APIActionResult<T> result, string message)
        {
            if (result == null)
            {
                result = new APIActionResult<T> { IsSuccess = false, ResultCode = "000002" };
            }
            result.ResultMessage = message;
            return result;
        }

        public static APIActionResult Opt(this APIActionResult result, string code, string message, object data)
        {
            if (result == null)
            {
                result = new APIActionResult();
            }
            if (code == "000000" || code == "111111")
            {
                result.IsSuccess = true;
            }
            result.Data = data;
            result.ResultCode = code;
            result.ResultMessage = message;
            return result;
        }

        public static APIActionResult Opt(this APIActionResult result, string code, string message)
        {
            if (result == null)
            {
                result = new APIActionResult();
            }
            if (code == "000000" || code == "111111")
            {
                result.IsSuccess = true;
            }
            result.ResultCode = code;
            result.ResultMessage = message;
            return result;
        }

        public static APIActionResult Opt(this APIActionResult result, string message)
        {
            if (result == null)
            {
                result = new APIActionResult { IsSuccess = false, ResultCode = "000002" };
            }
            result.ResultMessage = message;
            return result;
        }

        #endregion
    }

    /// <summary>
    /// 序列化
    /// </summary>
    public class StringUTF8Writer : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}