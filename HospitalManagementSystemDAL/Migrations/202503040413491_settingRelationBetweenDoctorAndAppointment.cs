namespace HospitalManagementSystemDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class settingRelationBetweenDoctorAndAppointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false, maxLength: 50),
                        PateintEmail = c.String(nullable: false),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        Symptoms = c.String(maxLength: 200),
                        AppointmentStatus = c.String(),
                        ConsultationDate = c.DateTime(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        Priority = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropColumn("dbo.AspNetUsers", "FullName");
            DropTable("dbo.Appointments");
        }
    }
}
