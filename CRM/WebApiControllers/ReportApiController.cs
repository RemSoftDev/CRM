using CRM.Report;
using System;
using System.IO;
using System.Web;
using System.Web.Http;
using Telerik.Reporting.Processing;

namespace CRM.WebApiControllers
{
	public class ReportApiController : ApiController
	{

		//GET: api/Reports/M
		public string Get(string id)
		{
			Telerik.Reporting.Report report = null;

			switch (id)
			{
				case "L":
					{
						report = new LeadsReport();
						break;
					}
				case "C":
					{
						report = new CustomerReport();
						break;
					}
				case "A":
					{
						report = new AdminTeamMemberReport();
						break;
					}
			}

			var path = CreatePath(id, report?.Name);


			CreatePdfFile(report, path.Item1);

			return path.Item2;
		}

		private void CreatePdfFile(Telerik.Reporting.Report report, string reportName)
		{
			var processor = new ReportProcessor();
			var result = processor.RenderReport("PDF", report, null);

			using (var pdfStream = new MemoryStream(result.DocumentBytes))
			using (var reportFile = new FileStream(Path.Combine(GetReportFilePath(), reportName), FileMode.Create))
			{
				pdfStream.CopyTo(reportFile);
			}
		}

		public Tuple<string, string> CreatePath(string id, string nameReport)
		{
			string reportFileName = $"{nameReport}.pdf";
			string reportWebPath = $"/Reports/{reportFileName}";

			return new Tuple<string, string>(reportFileName, reportWebPath);
		}

		private string GetReportFilePath()
		{
			string currentDirPath = HttpRuntime.AppDomainAppPath;
			string relativaPath = @"Reports";
			return Path.GetFullPath(Path.Combine(currentDirPath, relativaPath));
		}

	}
}
