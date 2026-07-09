using System;

namespace EasySQL
{
    public class FuncResult
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 附件对象。
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 获取函数执行结果是否成功。
        /// </summary>
        public bool Success => String.Equals(this.Code, "OK", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 构造函数。
        /// </summary>
        public FuncResult()
        {
            this.Code = "OK";
            this.Message = null;
            this.Tag = null;
        }

        public void SetError(string errorCode, string errorMessage)
        {
            this.Code = errorCode;
            this.Message = errorMessage;
        }

        public void SetError(string errorMessage)
        {
            this.SetError("ERROR", errorMessage);
        }
    }
}
