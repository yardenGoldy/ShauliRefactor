namespace ShauliProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Writer = c.String(nullable: false),
                        WriterWebSiteUrl = c.String(),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Writer = c.String(nullable: false),
                        WriterWebSiteUrl = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                        Image = c.String(),
                        Video = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Gender = c.Int(),
                        DateOfBirth = c.DateTime(),
                        YearsOfSeniority = c.Int(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostStats",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        Counter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostStats", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropIndex("dbo.PostStats", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropTable("dbo.PostStats");
            DropTable("dbo.Fans");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Admins");
        }
    }
}
