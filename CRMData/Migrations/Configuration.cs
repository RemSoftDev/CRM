using CRM.DAL.Contexts;

namespace CRM.DAL.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<CRM.DAL.Contexts.BaseContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(CRM.DAL.Contexts.BaseContext context)
		{
			CrmDbInitializer.InitDataInDB(context);
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
		}
	}
}
