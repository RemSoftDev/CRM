using CRM.Models;

namespace CRM.Services
{
	public interface ILogHistoryService
	{
		void Log(LogHistoryModel model);
	}
}