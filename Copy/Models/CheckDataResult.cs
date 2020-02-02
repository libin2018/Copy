using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Copy
{
    /// <summary>
    /// 检测模版，数据结果对象
    /// </summary>
    public class CheckDataResult
    {
        public bool Result { get; set; }

        public object Entity { get; set; }

        private IList<string> _errorMessages = new List<string>();

        public IList<string> ErrorMessages
        {
            get
            {
                return _errorMessages;
            }
        }
    }
}