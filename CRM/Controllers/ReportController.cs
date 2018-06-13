using CRM.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CRM.Controllers
{
	public class ReportController : Controller
	{
		// GET: Report
		public ActionResult Index()
		{
			ViewBag.ProductLines = new List<ProductLine>
			{
				new ProductLine {ProductLineCode = "M", ProductLineName = "Mountain"},
				new ProductLine {ProductLineCode = "R", ProductLineName = "Road"},
				new ProductLine {ProductLineCode = "S", ProductLineName = "Standard"},
				new ProductLine {ProductLineCode = "T", ProductLineName = "Touring"}
			};

			return View();
		}
	}
}