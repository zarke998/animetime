namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterRolesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CharacterRoles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Characters", "Role_Id", c => c.Int());
            CreateIndex("dbo.Characters", "Role_Id");
            AddForeignKey("dbo.Characters", "Role_Id", "dbo.CharacterRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Role_Id", "dbo.CharacterRoles");
            DropIndex("dbo.Characters", new[] { "Role_Id" });
            DropColumn("dbo.Characters", "Role_Id");
            DropTable("dbo.CharacterRoles");
        }
    }
}
