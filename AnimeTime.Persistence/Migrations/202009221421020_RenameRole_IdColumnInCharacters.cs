namespace AnimeTime.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameRole_IdColumnInCharacters : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Characters", "Role_Id", "dbo.CharacterRoles");
            DropIndex("dbo.Characters", new[] { "Role_Id" });
            RenameColumn(table: "dbo.Characters", name: "Role_Id", newName: "RoleId");
            AlterColumn("dbo.Characters", "RoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Characters", "RoleId");
            AddForeignKey("dbo.Characters", "RoleId", "dbo.CharacterRoles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "RoleId", "dbo.CharacterRoles");
            DropIndex("dbo.Characters", new[] { "RoleId" });
            AlterColumn("dbo.Characters", "RoleId", c => c.Int());
            RenameColumn(table: "dbo.Characters", name: "RoleId", newName: "Role_Id");
            CreateIndex("dbo.Characters", "Role_Id");
            AddForeignKey("dbo.Characters", "Role_Id", "dbo.CharacterRoles", "Id");
        }
    }
}
