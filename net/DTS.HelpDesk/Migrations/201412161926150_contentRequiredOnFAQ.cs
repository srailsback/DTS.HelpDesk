namespace DTS.HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contentRequiredOnFAQ : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FAQs", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FAQs", "Content", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
