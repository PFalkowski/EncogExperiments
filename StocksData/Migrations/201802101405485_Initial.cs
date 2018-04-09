namespace StocksData.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Ticker = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Ticker);
            
            CreateTable(
                "dbo.StockQuotes",
                c => new
                    {
                        Ticker = c.String(nullable: false, maxLength: 128),
                        Date = c.Int(nullable: false),
                        Open = c.Double(nullable: false),
                        High = c.Double(nullable: false),
                        Low = c.Double(nullable: false),
                        Close = c.Double(nullable: false),
                        Volume = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticker, t.Date })
                .ForeignKey("dbo.Companies", t => t.Ticker, cascadeDelete: true)
                .Index(t => t.Ticker);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockQuotes", "Ticker", "dbo.Companies");
            DropIndex("dbo.StockQuotes", new[] { "Ticker" });
            DropTable("dbo.StockQuotes");
            DropTable("dbo.Companies");
        }
    }
}
