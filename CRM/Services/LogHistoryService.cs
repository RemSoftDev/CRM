using CRM.Models;
using CRMData.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace CRM.Services
{
    public static class LogHistoryService
    {
        private const int maxListCount = 1000;
        private static List<LogHistoryModel> historyList = new List<LogHistoryModel>();

        public static void Log(LogHistoryModel model)
        {
            historyList.Add(model);
            if (historyList.Count >= maxListCount)
            {
                SaveHistory();
                historyList.TrimExcess();
            }
        }

        private static void SaveHistory()
        {
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {

            }
        }
    }
}