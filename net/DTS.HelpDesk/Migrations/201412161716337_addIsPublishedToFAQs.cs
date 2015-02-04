namespace DTS.HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsPublishedToFAQs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FAQs", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FAQs", "IsPublished");
        }
    }
}
