using EasySQL;

namespace EasySQL.Test
{
    public class OrderSchema : SchemaBase
    {
        public const string TABLE = "Orders";
        public const string ID = "Id";
        public const string USERID = "UserId";
        public const string ORDERNO = "OrderNo";
        public const string TOTALAMOUNT = "TotalAmount";
        public const string STATUS = "Status";
        public const string CREATETIME = "CreateTime";

        public override string TableName => TABLE;

        public OrderSchema(string alias, ISQLDialect? dialect) : base(alias, dialect) { }
        public OrderSchema(string alias) : this(alias, null) { }
        public OrderSchema() : this(string.Empty, null) { }

        public string Id => ID;
        public string UserId => USERID;
        public string OrderNo => ORDERNO;
        public string TotalAmount => TOTALAMOUNT;
        public string Status => STATUS;
        public string CreateTime => CREATETIME;

        public string GetId(bool p = true) => QuoteField(ID, p);
        public string GetUserId(bool p = true) => QuoteField(USERID, p);
        public string GetOrderNo(bool p = true) => QuoteField(ORDERNO, p);
        public string GetTotalAmount(bool p = true) => QuoteField(TOTALAMOUNT, p);
        public string GetStatus(bool p = true) => QuoteField(STATUS, p);
        public string GetCreateTime(bool p = true) => QuoteField(CREATETIME, p);
    }
}
