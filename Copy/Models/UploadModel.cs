using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Copy
{
    public class UploadModel
    {
        private string _id;
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id)) return string.Format("upload{0}", Guid.NewGuid().ToString().Replace("-", ""));

                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string _buttonName { get; set; }

        /// <summary>
        /// 按钮名称
        /// </summary>
        public string ButtonName
        {
            get
            {
                return _buttonName;
            }
            set
            {
                _buttonName = value;
            }
        }

        /// <summary>
        /// 上传到服务路径
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 文件扩展名（以.开头多个用，号分开）
        /// </summary>
        public string Extensions { get; set; }

        /// <summary>
        /// 回调函数
        /// </summary>
        public string CallBackFunction { get; set; }
    }
}