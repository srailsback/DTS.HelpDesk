namespace DTS.HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addArticles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        FriendlyTitle = c.String(),
                        Content = c.String(),
                        CreatedAtUTC = c.DateTime(nullable: false),
                        UpdatedAtUTC = c.DateTime(nullable: false),
                        PublishAtUTC = c.DateTime(nullable: false),
                        UnpublishAtUTC = c.DateTime(),
                        Clicks = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Articles", new[] { "UserId" });
            DropTable("dbo.Articles");
        }
    }
}
