using CRM.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using CRM.DAL.Entities;
using CRM.Report.DataProviders;

namespace CRM.Controllers
{
	public class ReportController : Controller
	{
		// GET: Report
		public ActionResult Index()
		{
			ViewBag.ProductLines = new List<ProductLine>
			{
				new ProductLine {ProductLineCode = "L", ProductLineName = "LeadReport"},

			};

			return View();
		}

		public ActionResult GenerateData()
		{
			var itemToCreateViewModel = new ItemToCreateViewModel();
			return View(itemToCreateViewModel);
		}

		[HttpPost]
		public ActionResult GenerateData(ItemToCreateViewModel generateDataViewModel)
		{
			var leadList = new List<Lead>();

			for (int i = 0; i < generateDataViewModel.Count; i++)
			{
				var name = Faker.Name.FullName();

				var lead = new Lead
				{
					Name = name,
					Email = Faker.Internet.Email(name),
				};

				leadList.Add(lead);
			}

			DataProvider<Lead>.SetData(leadList);

			return View("Index");
		}
	}
}