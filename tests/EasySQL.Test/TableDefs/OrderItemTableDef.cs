using EasySQL;

namespace EasySQL.Test
{
    public class OrderItemTableDef : TableDefBase
    {
        public const string TABLE = "OrderItems";
        public const string ID = "Id";
        public const string ORDERID = "OrderId";
        public const string PRODUCTID = "ProductId";
        public const string QUANTITY = "Quantity";
        public const string UNITPRICE = "UnitPrice";
        public const string CREATETIME = "CreateTime";

        public override string TableName => TABLE;

        public OrderItemTableDef(string alias, ISQLDialect? dialect) : base(alias, dialect) { }
        public OrderItemTableDef(string alias) : this(alias, null) { }
        public OrderItemTableDef() : this(string.Empty, null) { }

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
