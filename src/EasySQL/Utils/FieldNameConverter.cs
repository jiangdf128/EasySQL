using System;
using System.Collections.Concurrent;
using System.Text;

namespace EasySQL
{
    /// <summary>
    /// 字段命名约定转换工具。snake_case ↔ PascalCase，带静态缓存。
    /// </summary>
    internal static class FieldNameConverter
    {
        private static readonly ConcurrentDictionary<string, string> _snakeToPascalCache = new();

        /// <summary>
        /// snake_case → PascalCase。如 "user_name" → "UserName"。
        /// 无下划线的字符串首字母大写后返回。
        /// </summary>
        public static string SnakeToPascalCase(string snakeCase)
        {
            if (string.IsNullOrEmpty(snakeCase))
                return string.Empty;

            if (!snakeCase.Contains('_'))
            {
                // 无下划线：首字母大写（id → Id）
                return snakeCase.Length == 1
                    ? char.ToUpperInvariant(snakeCase[0]).ToString()
                    : char.ToUpperInvariant(snakeCase[0]) + snakeCase.Substring(1);
            }

            return _snakeToPascalCache.GetOrAdd(snakeCase, static s =>
            {
                var sb = new StringBuilder(s.Length);
                bool nextUpper = true;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '_')
                    {
                        nextUpper = true;
                    }
                    else
                    {
                        sb.Append(nextUpper ? char.ToUpperInvariant(s[i]) : s[i]);
                        nextUpper = false;
                    }
                }
                return sb.ToString();
            });
        }
    }
}
