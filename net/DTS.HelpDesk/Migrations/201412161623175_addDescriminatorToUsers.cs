namespace DTS.HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescriminatorToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "RequirePasswordReset", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "RequirePasswordReset", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "Discriminator");
        }
    }
}
