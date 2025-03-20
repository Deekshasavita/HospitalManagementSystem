namespace HospitalManagementSystemDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientEmailColumnChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "PatientEmail", c => c.String(nullable: false));
            DropColumn("dbo.Appointments", "PateintEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "PateintEmail", c => c.String(nullable: false));
            DropColumn("dbo.Appointments", "PatientEmail");
        }
    }
}
