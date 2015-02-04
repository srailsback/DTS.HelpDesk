namespace DTS.HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requirePasswordResetOnUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RequirePasswordReset", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RequirePasswordReset");
        }
    }
}
