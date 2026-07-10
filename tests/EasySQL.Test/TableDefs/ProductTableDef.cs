using EasySQL;

namespace EasySQL.Test
{
    public class ProductTableDef : TableDefBase
    {
        public const string TABLE = "Products";
        public const string ID = "Id";
        public const string NAME = "Name";
        public const string PRICE = "Price";
        public const string STOCK = "Stock";
        public const string STATUS = "Status";
        public const string CREATETIME = "CreateTime";

        public override string TableName => TABLE;

        public ProductTableDef(string alias, ISQLDialect? dialect) : base(alias, dialect) { }
        public ProductTableDef(string alias) : this(alias, null) { }
        public ProductTableDef() : this(string.Empty, null) { }

        public string Id => ID;
        public string Name => NAME;
        public string Price => PRICE;
        public string Stock => STOCK;
        public string Status => STATUS;
        public string CreateTime => CREATETIME;

        public string GetId(bool p = true) => QuoteField(ID, p);
        public string GetName(bool p = true) => QuoteField(NAME, p);
        public string GetPrice(bool p = true) => QuoteField(PRICE, p);
        public string GetStock(bool p = true) => QuoteField(STOCK, p);
        public string GetStatus(bool p = true) => QuoteField(STATUS, p);
        public string GetCreateTime(bool p = true) => QuoteField(CREATETIME, p);
    }
}
