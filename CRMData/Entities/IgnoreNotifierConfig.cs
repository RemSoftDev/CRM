using System;
using System.Collections.Generic;

namespace CRM.DAL.Entities
{
	public class IgnoreNotifierWorkDayConfig
	{
		public int Id { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public TimeSpan StartWorkTime { get; set; }
		public TimeSpan EndWorkTime { get; set; }
		public virtual IgnoreNotifierConfig IgnoreNotifierConfig { get; set; }

	}

	public class IgnoreNotifierConfig
	{
		public int Id { get; set; }
		public TimeSpan FirstDuration { get; set; }
		public TimeSpan SecondDuration { get; set; }
		public virtual ICollection<User> FirstRecipients { get; set; }
		public virtual ICollection<User> SecondRecipients { get; set; }
	}

}
