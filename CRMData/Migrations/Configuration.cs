using CRM.DAL.Contexts;

namespace CRM.DAL.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<BaseContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			ContextKey = "CRM.DAL.Contexts.BaseContext";
		}

		protected override void Seed(BaseContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
			CrmDbInitializer.InitDataInDB(context);

		}
	}
}
