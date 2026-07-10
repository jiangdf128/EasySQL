using EasySQL;

namespace EasySQL.Test
{
    public class OrderPaymentSchema : SchemaBase
    {
        public const string TABLE = "OrderPayments";
        public const string ID = "Id";
        public const string ORDERID = "OrderId";
        public const string PAYMETHOD = "PayMethod";
        public const string AMOUNT = "Amount";
        public const string PAYTIME = "PayTime";
        public const string STATUS = "Status";
        public const string CREATETIME = "CreateTime";

        public override string TableName => TABLE;

        public OrderPaymentSchema(string alias, ISQLDialect? dialect) : base(alias, dialect) { }
        public OrderPaymentSchema(string alias) : this(alias, null) { }
        public OrderPaymentSchema() : this(string.Empty, null) { }

        public string Id => ID;
        public string OrderId => ORDERID;
        public string PayMethod => PAYMETHOD;
        public string Amount => AMOUNT;
        public string PayTime => PAYTIME;
        public string Status => STATUS;
        public string CreateTime => CREATETIME;

        public string GetId(bool p = true) => QuoteField(ID, p);
        public string GetOrderId(bool p = true) => QuoteField(ORDERID, p);
        public string GetPayMethod(bool p = true) => QuoteField(PAYMETHOD, p);
        public string GetAmount(bool p = true) => QuoteField(AMOUNT, p);
        public string GetPayTime(bool p = true) => QuoteField(PAYTIME, p);
        public string GetStatus(bool p = true) => QuoteField(STATUS, p);
        public string GetCreateTime(bool p = true) => QuoteField(CREATETIME, p);
    }
}
