using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
	public class ReportController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ReportController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		// GET: Report
		public ActionResult Index()
		{
			ViewBag.ProductLines = new List<ProductLine>
			{
				new ProductLine {ProductLineCode = "L", ProductLineName = "LeadReport"},
				new ProductLine {ProductLineCode = "C", ProductLineName = "CustomerReport"},
				new ProductLine {ProductLineCode = "A", ProductLineName = "AdminTeamMemberReport"}
			};

			return View();
		}

		public ActionResult GenerateData()
		{
			var itemToCreateViewModel = new ItemToCreateViewModel();
			return View(itemToCreateViewModel);
		}

		[HttpPost]
		public ActionResult GenerateData(ItemToCreateViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var isSuccess = CreateFackeData(model);

			if (!isSuccess)
			{
				return View(model);
			}

			_unitOfWork.Save();

			return RedirectToAction(nameof(Index));
		}

		public bool CreateFackeData(ItemToCreateViewModel generateData)
		{
			var isSuccess = false;

			switch (generateData.ItemType)
			{
				case ItemType.Leads:
					{
						var leads = GenerateLeads(generateData.Count);
						_unitOfWork.LeadsRepository.AddRange(leads);
						isSuccess = true;
						break;
					}

				case ItemType.Customer:
					{
						var customers = GenerateUser(generateData.Count, _unitOfWork.DUserTypesRepository.Customer);
						_unitOfWork.UsersRepository.AddRange(customers);
						isSuccess = true;
						break;
					}

				case ItemType.AdminTeamMember:
					{
						var customers = GenerateUser(generateData.Count, _unitOfWork.DUserTypesRepository.AdminTeamMember);
						_unitOfWork.UsersRepository.AddRange(customers);
						isSuccess = true;
						break;
					}
			}

			return isSuccess;
		}

		private IEnumerable<User> GenerateUser(int count, DUserType userType)
		{
			var userList = new List<User>();

			for (int i = 0; i < count; i++)
			{
				var name = Faker.Name.FullName();

				var lead = new User()
				{
					FirstName = Faker.Name.First(),
					LastName = Faker.Name.Last(),
					Email = Faker.Internet.Email(name),
					UserType = userType,
					Phones = GeneratePhones().ToList(),
					Addresses = GenerateAddress().ToList()
				};

				userList.Add(lead);
			}

			return userList;
		}

		private IEnumerable<Lead> GenerateLeads(int count)
		{
			var leadList = new List<Lead>();

			for (int i = 0; i < count; i++)
			{
				var name = Faker.Name.FullName();

				var lead = new Lead
				{
					Name = name,
					Email = Faker.Internet.Email(name),
					Phones = GeneratePhones().ToList(),
				};

				leadList.Add(lead);
			}

			return leadList;
		}

		private IEnumerable<Address> GenerateAddress()
		{
			yield return new Address()
			{
				Country = Faker.Address.Country(),
				Town = Faker.Address.City(),
			};
		}

		private IEnumerable<Phone> GeneratePhones()
		{
			yield return new Phone()
			{
				PhoneNumber = Faker.Phone.Number()
			};
		}
	}
}