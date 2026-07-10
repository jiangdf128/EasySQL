using EasySQL;

namespace EasySQL.Test
{
    public class OrderItemSchema : SchemaBase
    {
        public const string TABLE = "OrderItems";
        public const string ID = "Id";
        public const string ORDERID = "OrderId";
        public const string PRODUCTID = "ProductId";
        public const string QUANTITY = "Quantity";
        public const string UNITPRICE = "UnitPrice";
        public const string CREATETIME = "CreateTime";

        public override string TableName => TABLE;

        public OrderItemSchema(string alias, ISQLDialect? dialect) : base(alias, dialect) { }
        public OrderItemSchema(string alias) : this(alias, null) { }
        public OrderItemSchema() : this(string.Empty, null) { }

        public string Id => ID;
        public string OrderId => ORDERID;
        public string ProductId => PRODUCTID;
        public string Quantity => QUANTITY;
        public string UnitPrice => UNITPRICE;
        public string CreateTime => CREATETIME;

        public string GetId(bool p = true) => QuoteField(ID, p);
        public string GetOrderId(bool p = true) => QuoteField(ORDERID, p);
        public string GetProductId(bool p = true) => QuoteField(PRODUCTID, p);
        public string GetQuantity(bool p = true) => QuoteField(QUANTITY, p);
        public string GetUnitPrice(bool p = true) => QuoteField(UNITPRICE, p);
        public string GetCreateTime(bool p = true) => QuoteField(CREATETIME, p);
    }
}
