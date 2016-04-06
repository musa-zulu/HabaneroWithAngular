using FluentMigrator;

namespace TestHabanero.Migrations.Migrations
{
    [Migration(201603291029)]
    public class _201603291029_CreateCarPartsTable : Migration
    {
        public override void Up()
        {
            Create.Table("CarPart")
                .WithColumn("CarPartId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("CarId").AsGuid().NotNullable().ForeignKey("FK_Car_CarParts","Car","CarId")
                .WithColumn("PartId").AsGuid().NotNullable().ForeignKey("FK_Parts_CarParts","Part","PartId")
                .WithColumn("Quantity").AsInt32().NotNullable();

        }

        public override void Down()
        {

        }
    }
}