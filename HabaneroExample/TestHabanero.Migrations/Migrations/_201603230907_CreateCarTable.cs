using FluentMigrator;

namespace TestHabanero.Migrations.Migrations
{
    [Migration(201603230907)]
    public class _201603230907_CreateCarTable: Migration
    {
        public override void Up()
        {
            Create.Table("Car")
                .WithColumn("CarId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Make").AsString(255).Nullable()
                .WithColumn("Color").AsString(255).Nullable()
                .WithColumn("Model").AsString(255).Nullable();

        }

        public override void Down()
        {
           
        }
    }
}