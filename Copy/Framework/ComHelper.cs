using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Copy
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class ComHelper
    {
        /// <summary>
        /// HttpClient
        /// </summary>
        public static HttpClient HClient { get; } = new HttpClient();

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            byte[] bytes = Encoding.UTF8.GetBytes(str);

            MemoryStream stream = new MemoryStream();

            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);

            stream2.Write(bytes, 0, bytes.Length);

            stream2.FlushFinalBlock();

            StringBuilder builder = new StringBuilder();

            foreach (byte num in stream.ToArray())

            {

                builder.AppendFormat("{0:X2}", num);

            }

            stream.Close();

            return builder.ToString();

        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decode(string str, string key)

        {

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            byte[] buffer = new byte[str.Length / 2];

            for (int i = 0; i < (str.Length / 2); i++)

            {

                int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);

                buffer[i] = (byte)num2;

            }

            MemoryStream stream = new MemoryStream();

            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);

            stream2.Write(buffer, 0, buffer.Length);

            stream2.FlushFinalBlock();

            stream.Close();

            return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
        }

        /// <summary>
        /// 检查是否包含列值
        /// </summary>
        /// <param name="key">列</param>
        /// <param name="cols">集合</param>
        /// <returns></returns>
        public static bool CheckKeyInCols(string key, params string[] cols)
        {
            var list = cols.ToList();
            return list.Any(t => string.Compare(key, t, true) == 0);
        }

        /// <summary>
        /// 检查是不是手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^[1]+[3,4,5,6,7,8,9]+\d{9}");
        }

        /// <summary>
        /// 大小写转换
        /// </summary>
        /// <param name="nmoney"></param>
        /// <returns></returns>
        public static string MoneyChanageToUpper(double? nmoney)
        {
            if (nmoney.HasValue)
            {
                string money = Math.Abs(nmoney.Value).ToString("f2");
                String[] MyScale = { "分", "角", "元", "", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
                String[] MyBase = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
                String M = nmoney < 0 ? "负" : string.Empty;

                var tmp = string.Empty;
                for (int i = 0; i < money.Length; i++)
                {
                    if (money[i] == '.')
                    {
                        M += "元";
                        tmp = string.Empty;
                    }
                    else
                    {
                        int MyData = Convert.ToInt16(money[i].ToString());
                        if (MyData > 0)
                        {
                            M += tmp + MyBase[MyData];
                            M += MyScale[(money.Length - 1) - i];
                            tmp = string.Empty;
                        }
                        else
                        {
                            tmp = MyBase[0];
                        }
                    }
                }
                return M;
            }
            return string.Empty;
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DateToString(DateTime? date, string format = "yyyy-MM-dd")
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format);
            }
            return string.Empty;
        }

        public static string DateToStringCH(DateTime? date, string format = "yyyy年MM月dd日hh时mm分")
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format);
            }
            return string.Empty;
        }

        /// <summary>
        /// 金额转换,默认为ToString("f2")
        /// </summary>
        /// <param name="val"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DoubleToString(double? val, string format = "f2")
        {
            if (val.HasValue)
            {
                return val.Value.ToString(format);
            }
            return string.Empty;
        }

        /// <summary>
        /// 金额转换,默认为ToString("f2")
        /// </summary>
        /// <param name="val"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DecimalToString(decimal? val, string format = "f2")
        {
            if (val.HasValue)
            {
                return val.Value.ToString(format);
            }
            return string.Empty;
        }

        public static double StringToDouble(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                try
                {
                    return double.Parse(val);
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        public static int StringToInt(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                try
                {
                    return int.Parse(val);
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// 返回手机所在地
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static dynamic GetMobileArea(string mobile)
        {
            var url = string.Format("https://cx.shouji.360.cn/phonearea.php?number={0}", mobile);
            var str = HClient.GetStringAsync(url).Result;
            var json = JObject.Parse(HttpUtility.UrlDecode(str));
            var o = json.GetValue("data") as JObject;
            return new AreaRet { Mobile = mobile, Province = o.GetJValue("province"), City = o.GetJValue("city"), Sp = o.GetJValue("sp") };
        }

        /// <summary>
        /// 手机所在地
        /// </summary>
        public class AreaRet
        {
            /// <summary>
            /// 手机号码
            /// </summary>
            public string Mobile { get; set; }

            /// <summary>
            /// 市   
            /// </summary>
            public string Province { get; set; }

            /// <summary>
            /// 省       
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// 运营商
            /// </summary>
            public string Sp { get; set; }
        }

        /// <summary>
        /// 检查数据是否存在：去空格，不区分大小写
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsStringExists(string key, params string[] list)
        {
            var tmp = list.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim().ToLower()).ToList();
            return !string.IsNullOrEmpty(key) && tmp.Contains(key.Trim().ToLower());
        }

        public static bool CheckJTokenIsNull(JObject json, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (json.GetValue(key, StringComparison.InvariantCultureIgnoreCase) == null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 读取SSO站点地址
        /// </summary>
        /// <returns></returns>
        public static string GetSSOCheckUrl()
        {
            var request = System.Web.HttpContext.Current.Request;
            return HttpUtility.UrlEncode(string.Format("{0}://{1}/sso/check", request.Url.Scheme, request.Headers["host"]));
        }

        /// <summary>
        /// 读取SSO站点地址
        /// </summary>
        /// <returns></returns>
        public static string BuildSSOCheckUrl(string path)
        {
            var ext = path.Contains("?") ? "&" : "?";
            var request = System.Web.HttpContext.Current.Request;
            var ssoUrl = HttpUtility.UrlEncode(string.Format("{0}://{1}/sso/check", request.Url.Scheme, request.Headers["host"]));
            return path + ext + "sso=" + ssoUrl;
        }

        public static int MonthDiff(DateTime date1, DateTime date2)
        {
            int months = 0;
            if (date2 <= date1)  //如果日期1大于日期2返回0
            {
                return months;
            }
            if (date2.Month >= date1.Month)
            {
                months = date2.Month - date1.Month; //月份差
                months = months + (date2.Year - date1.Year) * 12;  //年份差*12
            }
            else
            {
                months = 12 - date1.Month;
                months = months + date2.Month;
                months = months + (date2.Year - 1 - date1.Year) * 12;
            }
            return months;
        }

        /// <summary>
        /// 是否为数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static string GetIP()
        {
            var ip = "";
            try
            {
                var req = System.Web.HttpContext.Current.Request;
                ip = req.ServerVariables["HTTP_VIA"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }

                if (string.IsNullOrEmpty(ip))
                {
                    ip = req.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(ip))
                {
                    ip = req.UserHostAddress;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Log.Error(ex);
            }
            return ip;
        }
        
        private const string MSContextKey = "MS_HttpContext";

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIP(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(MSContextKey))
            {
                HttpContextBase context = (HttpContextBase)request.Properties[MSContextKey];        // 获取传统context
                return GetIP(context.Request);
            }
            return string.Empty;
        }

        /// <summary>
        /// 读取IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIP(HttpRequestBase request)
        {
            string result = string.Empty;
            result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = request.UserHostAddress;
            }

            if (null == result || result == String.Empty || !IsIP(result))
            {
                result = "0.0.0.0";
            }

            return result;
        }

        /// <summary>
        /// 是否为IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string JsonSerialize(object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, new Newtonsoft.Json.JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }


        /// <summary>
        /// 大小写转换
        /// </summary>
        /// <param name="nmoney"></param>
        /// <returns></returns>
        public static string MoneyChanageToUpper(decimal? nmoney)
        {
            if (nmoney.HasValue)
            {
                string money = Math.Abs(nmoney.Value).ToString("f2");
                String[] MyScale = { "分", "角", "元", "", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
                String[] MyBase = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
                String M = nmoney < 0 ? "负" : string.Empty;

                var tmp = string.Empty;
                for (int i = 0; i < money.Length; i++)
                {
                    if (money[i] == '.')
                    {
                        M += "元";
                        tmp = string.Empty;
                    }
                    else
                    {
                        int MyData = Convert.ToInt16(money[i].ToString());
                        if (MyData > 0)
                        {
                            M += tmp + MyBase[MyData];
                            M += MyScale[(money.Length - 1) - i];
                            tmp = string.Empty;
                        }
                        else
                        {
                            tmp = MyBase[0];
                        }
                    }
                }
                return M;
            }
            return string.Empty;
        }

        /// <summary>
        /// 数据值转字符串
        /// </summary>
        /// <param name="val"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DoubleToString(decimal? val, string format = "f2")
        {
            if (val.HasValue)
            {
                return val.Value.ToString(format);
            }
            return string.Empty;
        }

        public static string IntToString(int? val)
        {
            return val != null ? val.ToString() : string.Empty;
        }

        /// <summary>
        /// 去空格
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Trim(string val)
        {
            return string.IsNullOrEmpty(val) ? "" : val.Trim();
        }

        /// <summary>
        /// 生成完整地址
        /// </summary>
        /// <param name="strUrl">strUrl</param>
        /// <returns>string</returns>
        public static string GetAbsoluteUrl(string strUrl)
        {
            if (!string.IsNullOrEmpty(strUrl))
            {
                var urll = new Uri(HttpContext.Current.Request.Url, VirtualPathUtility.ToAbsolute(strUrl)).AbsoluteUri;
                return urll;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取虚拟路径
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string GetVirPath(string strUrl)
        {
            if (!string.IsNullOrEmpty(strUrl))
            {
                var urll = HttpContext.Current.Request.MapPath(strUrl);
                return urll;
            }
            return string.Empty;
        }

        /**
        * @将Dictionary转成xml
        * @return 经转换得到的xml串
        * @throws WxPayException
        **/
        public static string DicToXml(Dictionary<string, object> m_values)
        {
            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    throw new Exception("内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else
                {
                    throw new Exception("字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /// <summary>
        /// 生成指定长度的随机数
        /// </summary>
        /// <returns></returns>
        public static string GetRandom(int length)
        {
            Random Random = new Random(DateTime.Now.Second);
            var Vcode = "";
            for (int i = 1; i <= length; i++)
            {
                Vcode += Random.Next(1, 9);
            }
            return Vcode;
        }

        /// <summary>
        /// 正数分解成2的幂方集合
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<int> GetDecompose(int number)
        {
            var decomlist = new List<int>();
            var templist = new List<int>();
            while (number != 0)
            {
                var remainder = number % 2;
                number = number / 2;
                templist.Add(remainder);
            }
            for (int i = 0; i < templist.Count; i++)
            {
                if (templist[i] == 1)
                {
                    var freNum = Frequency(2, i);
                    decomlist.Add(freNum);
                }
            }
            return decomlist;
        }

        /// <summary>
        /// 获取x的y幂方
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int Frequency(int x, int y)
        {
            int a = 1;
            for (int i = 1; i <= y; i++)
            {
                a = a * x;
            }
            return a;
        }
        /// <summary>
        /// 设置Int
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        public static void Set2Int(IDictionary<String, Object> item, string key)
        {
            object val;
            if (item.TryGetValue(key, out val))
            {
                item[key] = val != null ? (Convert.ToDouble(val).ToString("F2")) : string.Empty;
            }
        }
        /// <summary>
        /// 设置百分比
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        public static void SetPensnt(IDictionary<String, Object> item, string key)
        {
            object val;
            if (item.TryGetValue(key, out val))
            {
                item[key] = val != null ? (Convert.ToDouble(val) * 100).ToString("F2") + "%" : string.Empty;
            }
        }

        private static string SetPensnt(double? a, double? b)
        {
            if (a != null && b != null && b > 0)
            {
                return (a.Value * 100 / b.Value).ToString("F2") + "%";
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回当前的年份
        /// </summary>
        /// <returns></returns>
        public static int CurrentYear
        {
            get
            {
                return DateTime.Now.Year;
            }
        }

        /// <summary>
        /// 返回当前的月份
        /// </summary>
        /// <returns></returns>
        public static int CurrentMonth
        {
            get
            {
                return DateTime.Now.Month;
            }
        }

        /// <summary>
        /// 根据间隔获取距离间隔前后的年份
        /// </summary>
        /// <param name="inter"></param>
        /// <returns></returns>
        public static List<KeyValuePair<int, string>> GetLatelyYears(int inter)
        {
            List<KeyValuePair<int, string>> lstYears = new List<KeyValuePair<int, string>>();

            var dateNow = DateTime.Now;
            lstYears.Add(new KeyValuePair<int, string>(dateNow.Year, dateNow.Year + " 年"));
            for (int i = 1; i <= inter; i++)
            {
                lstYears.Add(new KeyValuePair<int, string>(dateNow.AddYears(-i).Year, dateNow.AddYears(-i).Year + " 年"));
                lstYears.Add(new KeyValuePair<int, string>(dateNow.AddYears(i).Year, dateNow.AddYears(i).Year + " 年"));
            }

            lstYears.Sort((item1, item2) =>
            {
                if (item1.Key > item2.Key)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            });

            return lstYears;
        }
    }
}