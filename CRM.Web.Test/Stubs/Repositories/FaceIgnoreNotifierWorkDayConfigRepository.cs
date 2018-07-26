using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CRM.DAL.Entities;
using CRM.DAL.Repository;

namespace CRM.Web.Test.Stubs.Repositories
{
	public class FaceIgnoreNotifierWorkDayConfigRepository : IGenericRepository<IgnoreNotifierWorkDayConfig>
	{
		public void Create(IgnoreNotifierWorkDayConfig item)
		{
			throw new NotImplementedException();
		}

		public IgnoreNotifierWorkDayConfig FindById(int id)
		{
			throw new NotImplementedException();
		}

		public IgnoreNotifierWorkDayConfig FindBy(Func<IgnoreNotifierWorkDayConfig, bool> predicate)
		{
			return new IgnoreNotifierWorkDayConfig
			{
				DayOfWeek = DayOfWeek.Monday,
				StartWorkTime = new TimeSpan(9, 0, 0),
				EndWorkTime = new TimeSpan(18, 0, 0),
				IgnoreNotifierConfig = new IgnoreNotifierConfig
				{
					FirstDuration = new TimeSpan(0, 15, 0),
					SecondDuration = new TimeSpan(1, 0, 0),
				}
			};
		}

		public IEnumerable<IgnoreNotifierWorkDayConfig> Get()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IgnoreNotifierWorkDayConfig> Get(Func<IgnoreNotifierWorkDayConfig, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public void Remove(IgnoreNotifierWorkDayConfig item)
		{
			throw new NotImplementedException();
		}

		public void Update(IgnoreNotifierWorkDayConfig item)
		{
			throw new NotImplementedException();
		}

		public bool Any(Func<IgnoreNotifierWorkDayConfig, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public void UpdateRange(IEnumerable<IgnoreNotifierWorkDayConfig> items)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IgnoreNotifierWorkDayConfig> GetWithInclude(Func<IgnoreNotifierWorkDayConfig, bool> predicate, params Expression<Func<IgnoreNotifierWorkDayConfig, object>>[] includeProperties)
		{
			throw new NotImplementedException();
		}

		public void AddRange(IEnumerable<IgnoreNotifierWorkDayConfig> entities)
		{
			throw new NotImplementedException();
		}
	}
}
