using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddNlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TABLE `nlog` (
                  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
                  `Application` varchar(50) DEFAULT NULL,
                  `Logged` datetime DEFAULT NULL,
                  `Level` varchar(50) DEFAULT NULL,
                  `Message` varchar(512) DEFAULT NULL,
                  `Logger` varchar(250) DEFAULT NULL,
                  `Callsite` varchar(512) DEFAULT NULL,
                  `Exception` varchar(512) DEFAULT NULL,
                  PRIMARY KEY (`Id`)
                ) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE nlog;");
        }
    }
}