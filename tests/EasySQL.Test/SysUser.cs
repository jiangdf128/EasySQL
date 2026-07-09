using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;

namespace EasySQL.Test
{
    [Table("cxd_users")]
    public class SysUser
    {
        public int? Id { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public short? MemberType { get; set; }
        public short Status { get; set; }
        public long ExtOption { get; set; }
        public DateTime CreateTime { get; set; }
        public int Version { get; set; }
    }
}
