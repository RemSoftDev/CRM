using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Report;
using Telerik.Reporting.Processing;

namespace CRM.WebApiControllers
{
    public class ReportApiController : ApiController
    {
	    private const string ReportsPath = @"D:\Work\CRM\Project\CRM\Reports";

	    //GET: api/Reports/M
	    public string Get(string id)
	    {
		    string reportFileName = String.Format("Products-{0}.pdf", id);
		    string reportWebPath = String.Format("/Reports/{0}", reportFileName);

		    if (File.Exists(Path.Combine(ReportsPath, reportFileName)))
		    {
			    return reportWebPath;
		    }

		    var productListReport = new Report1();
		   // productListReport.ReportParameters["ProductLine"].Value = id;
		    var processor = new ReportProcessor();
		    var result = processor.RenderReport("PDF", productListReport, null);

		    using (var pdfStream = new MemoryStream(result.DocumentBytes))
		    using (var reportFile = new FileStream(Path.Combine(ReportsPath, reportFileName), FileMode.Create))
		    {
			    pdfStream.CopyTo(reportFile);
		    }

		    return reportWebPath;
	    }
	}
}
