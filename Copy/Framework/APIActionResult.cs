using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copy
{
    /// <summary>
    /// 对外返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("请使用APIResult")]
    public class APIActionResult<T>
    {
        /// <summary>
        /// 000000:表示正确/通过  111111:表示验证不通过/驳回   222222:表示事务执行出错
        /// </summary>
        public string ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public bool IsSuccess { get; set; }

        public T Data { get; set; }

        public string DataStr { get; set; }
    }

    [Obsolete("请使用APIResult")]
    public class APIActionResult : APIActionResult<object>
    {
        public APIActionResult() { }

        public APIActionResult(string code, string message)
        {
            this.ResultCode = code;
            this.IsSuccess = code == "000000" || code == "111111";
            this.ResultMessage = message;
        }

        public APIActionResult(string code, string message, bool istrue)
        {
            this.ResultCode = code;
            this.IsSuccess = istrue;
            this.ResultMessage = message;
        }

        public APIActionResult(string code, string message, object data, bool istrue)
        {
            this.ResultCode = code;
            this.IsSuccess = istrue;
            this.Data = data;
            this.ResultMessage = message;
        }

        public APIActionResult(string code, string message, object data, bool istrue, string DataStr)
        {
            this.ResultCode = code;
            this.IsSuccess = istrue;
            this.Data = data;
            this.ResultMessage = message;
            this.DataStr = DataStr;
        }
    }
}