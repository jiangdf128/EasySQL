using System;

namespace EasySQL
{
    /// <summary>
    /// 通用函数执行结果类，用于封装操作状态和错误信息。
    /// 默认状态为成功（Code = "OK"），可通过 <see cref="SetError(string)"/> 或 <see cref="SetError(string, string)"/> 标记为失败。
    /// </summary>
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

        /// <summary>
        /// 设置错误代码和错误消息，将状态标记为失败。
        /// </summary>
        /// <param name="errorCode">错误代码。</param>
        /// <param name="errorMessage">错误消息。</param>
        public void SetError(string errorCode, string errorMessage)
        {
            this.Code = errorCode;
            this.Message = errorMessage;
        }

        /// <summary>
        /// 设置错误消息，使用默认错误代码 "ERROR"。
        /// </summary>
        /// <param name="errorMessage">错误消息。</param>
        public void SetError(string errorMessage)
        {
            this.SetError("ERROR", errorMessage);
        }
    }
}
