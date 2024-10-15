namespace TS.Data.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        SetType = c.Int(nullable: false),
                        User_who_answer_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StatisticAnswers",
                c => new
                    {
                        QuestionsID = c.Guid(nullable: false),
                        AnswersID = c.Guid(nullable: false),
                        answers = c.String(),
                        value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionsID, t.AnswersID })
                .ForeignKey("dbo.Answers", t => t.AnswersID, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionsID, cascadeDelete: true)
                .Index(t => t.QuestionsID)
                .Index(t => t.AnswersID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        GroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.GroupID })
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(nullable: false),
                        Password = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Enable = c.Boolean(nullable: false),
                        Image = c.Binary(),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserUniversities",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        UniversityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.UniversityID })
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Universities", t => t.UniversityID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.UniversityID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        QuestionName = c.String(),
                        SetType = c.Int(nullable: false),
                        A1 = c.String(),
                        A2 = c.String(),
                        A3 = c.String(),
                        A4 = c.String(),
                        A5 = c.String(),
                        V1 = c.Int(nullable: false),
                        V2 = c.Int(nullable: false),
                        V3 = c.Int(nullable: false),
                        V4 = c.Int(nullable: false),
                        V5 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StatisticQuestions",
                c => new
                    {
                        StatisticsID = c.Guid(nullable: false),
                        QuestionsID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatisticsID, t.QuestionsID })
                .ForeignKey("dbo.Questions", t => t.QuestionsID, cascadeDelete: true)
                .ForeignKey("dbo.Statistics", t => t.StatisticsID, cascadeDelete: true)
                .Index(t => t.StatisticsID)
                .Index(t => t.QuestionsID);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        AddDate = c.DateTime(),
                        SetType = c.Int(nullable: false),
                        UserID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StudentsTargets",
                c => new
                    {
                        StatisticsID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatisticsID, t.UserID })
                .ForeignKey("dbo.Statistics", t => t.StatisticsID, cascadeDelete: true)
                .Index(t => t.StatisticsID);
            
            CreateTable(
                "dbo.Universities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Parent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Universities", t => t.Parent_ID)
                .Index(t => t.Parent_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserUniversities", "UniversityID", "dbo.Universities");
            DropForeignKey("dbo.Universities", "Parent_ID", "dbo.Universities");
            DropForeignKey("dbo.StudentsTargets", "StatisticsID", "dbo.Statistics");
            DropForeignKey("dbo.StatisticQuestions", "StatisticsID", "dbo.Statistics");
            DropForeignKey("dbo.StatisticQuestions", "QuestionsID", "dbo.Questions");
            DropForeignKey("dbo.StatisticAnswers", "QuestionsID", "dbo.Questions");
            DropForeignKey("dbo.UserUniversities", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.StatisticAnswers", "AnswersID", "dbo.Answers");
            DropIndex("dbo.Universities", new[] { "Parent_ID" });
            DropIndex("dbo.StudentsTargets", new[] { "StatisticsID" });
            DropIndex("dbo.StatisticQuestions", new[] { "QuestionsID" });
            DropIndex("dbo.StatisticQuestions", new[] { "StatisticsID" });
            DropIndex("dbo.UserUniversities", new[] { "UniversityID" });
            DropIndex("dbo.UserUniversities", new[] { "UserID" });
            DropIndex("dbo.UserGroups", new[] { "GroupID" });
            DropIndex("dbo.UserGroups", new[] { "UserID" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.StatisticAnswers", new[] { "AnswersID" });
            DropIndex("dbo.StatisticAnswers", new[] { "QuestionsID" });
            DropTable("dbo.Universities");
            DropTable("dbo.StudentsTargets");
            DropTable("dbo.Statistics");
            DropTable("dbo.StatisticQuestions");
            DropTable("dbo.Questions");
            DropTable("dbo.UserUniversities");
            DropTable("dbo.Users");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.StatisticAnswers");
            DropTable("dbo.Answers");
        }
    }
}
