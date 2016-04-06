using FluentMigrator;

namespace TestHabanero.Migrations.Migrations
{
    [Migration(29032016843)]
    public class _29032016843_CreatePartsTable: Migration
    {
        public override void Up()
        {
            Create.Table("Part")
                .WithColumn("PartId").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("Description").AsString(255).Nullable()
                .WithColumn("Price").AsInt32().Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}