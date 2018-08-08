using CRM.DAL.Entities;
using System;
using System.Data.Entity;

namespace CRM.DAL.Contexts
{
	public class CrmDbInitializer : CreateDatabaseIfNotExists<BaseContext>
	{
		protected override void Seed(BaseContext context)
		{
			context.DPhoneTypes.Add(new DPhoneType() { TypeName = "HomePhone" });
			context.DPhoneTypes.Add(new DPhoneType() { TypeName = "WorkPhone" });
			context.DPhoneTypes.Add(new DPhoneType() { TypeName = "MobilePhone" });
			context.DPhoneTypes.Add(new DPhoneType() { TypeName = "EmergencyContactPhone" });
			context.DPhoneTypes.Add(new DPhoneType() { TypeName = "Fax" });

			context.DAddressTypes.Add(new DAddressType() { TypeName = "BillingAddress" });
			context.DAddressTypes.Add(new DAddressType() { TypeName = "ContactAddress" });
			context.DAddressTypes.Add(new DAddressType() { TypeName = "EmergencyContactAddress" });

			context.DUserTypes.Add(new DUserType() { TypeName = "AdminTeamMember" });
			context.DUserTypes.Add(new DUserType() { TypeName = "Customer" });

			SeedIgnoreNotifierConfig(context);

			base.Seed(context);
		}

		private void SeedIgnoreNotifierConfig(BaseContext context)
		{
			var ignoreNotifierConfig = new IgnoreNotifierConfig
			{
				FirstDuration = new TimeSpan(0, 15, 0),
				SecondDuration = new TimeSpan(1, 0, 0),
				//EmailSubject = "Ignore Config",
				//EmailBody = "Test"
			};
			var ignoreNotifierMondayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Monday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			var ignoreNotifierTuesdayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Tuesday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			var ignoreNotifierWednesdayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Wednesday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			var ignoreNotifierThursdayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Thursday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			var ignoreNotifierFridayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Friday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			var ignoreNotifierSaturdayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Saturday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			var ignoreNotifierSundayConfig = new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Sunday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = ignoreNotifierConfig
			};

			context.IgnoreNotifierConfigs.Add(ignoreNotifierConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierMondayConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierTuesdayConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierWednesdayConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierThursdayConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierFridayConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierSaturdayConfig);
			context.IgnoreNotifierWorkDayConfigs.Add(ignoreNotifierSundayConfig);
		}

		public static void InitDataInDB(BaseContext db)
		{
			new CrmDbInitializer().Seed(db);
		}
	}
}