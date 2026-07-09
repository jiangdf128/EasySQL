using System;
using System.Collections.Generic;
using System.Text;

namespace EasySQL.Test
{
    public class SqlComment
    {
        public string TableName { get; set; }

        public int Line { get; set; }

        public string Comment { get; set; }

        public string Type { get; set; }

        public string ColName { get; set; }
    }
}
