using EasySQL;

namespace EasySQL.Test
{
    public class UserTableDef : TableDefBase
    {
        public const string TABLE = "Users";
        public const string ID = "Id";
        public const string USERNAME = "UserName";
        public const string EMAIL = "Email";
        public const string PHONE = "Phone";
        public const string BALANCE = "Balance";
        public const string STATUS = "Status";
        public const string CREATETIME = "CreateTime";

        public override string TableName => TABLE;

        public UserTableDef(string alias, ISQLDialect? dialect) : base(alias, dialect) { }
        public UserTableDef(string alias) : this(alias, null) { }
        public UserTableDef() : this(string.Empty, null) { }

        public string Id => ID;
        public string UserName => USERNAME;
        public string Email => EMAIL;
        public string Phone => PHONE;
        public string Balance => BALANCE;
        public string Status => STATUS;
        public string CreateTime => CREATETIME;

        public string GetId(bool p = true) => QuoteField(ID, p);
        public string GetUserName(bool p = true) => QuoteField(USERNAME, p);
        public string GetEmail(bool p = true) => QuoteField(EMAIL, p);
        public string GetPhone(bool p = true) => QuoteField(PHONE, p);
        public string GetBalance(bool p = true) => QuoteField(BALANCE, p);
        public string GetStatus(bool p = true) => QuoteField(STATUS, p);
        public string GetCreateTime(bool p = true) => QuoteField(CREATETIME, p);
    }
}
