using AutoMapper;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Models;
using CRM.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Client;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRM.WebApiControllers
{
    public class WebFormController : ApiController
	{
        private const string LEAD_GROUP_NAME = "lead";

        private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailService _emailService;
        private readonly IHubContext _hubContext;

        public WebFormController(IUnitOfWork unitOfWork, 
            IEmailService emailService, 
            IHubContext hubContext)
		{
			_unitOfWork = unitOfWork;
			_emailService = emailService;
            _hubContext = hubContext;
		}

		[HttpPost]
		public HttpResponseMessage CreateLead(
            [FromBody]LeadViewModel model)
		{
            var phone = model?.Phones?.FirstOrDefault();
			if (phone != null)
			{
				phone.Type = Enums.PhoneType.HomePhone;
			}

			if (ModelState.IsValid)
			{
                // Set IsSaved = false mean that the lead comes from POST request and need to be edited/converted
                Lead lead = Mapper.Map<LeadViewModel, Lead>(model);
				lead.IsConverted = false;
                lead.IsSaved = false;

				if (_unitOfWork.LeadsRepository.FindBy(l => l.Email == lead.Email) == null)
				{
					_unitOfWork.LeadsRepository.Create(lead);
					_unitOfWork.Save();

                    // havent work yet
                    //_hubContext.Clients.All.onResetPopUp();
                    //
                    //var connection = new HubConnection("");
                    //var mapResult = Mapper.Map<PopUpViewModel>(lead);
                    //
                    //IHubProxy myHub = connection.CreateHubProxy("phoneHub");
                    //connection.Start().Wait();
                    //myHub.("onResetPopUp", mapResult);

                    model.Id = lead.Id;

					if (!string.IsNullOrEmpty(model.Message))
					{
						EmailViewModel email = new EmailViewModel
						{
							To = "csharpcrm@gmail.com",
							Subject = "Lead Registered",
							Body = model.Message,
							WasReceived = true
						};

						new Task(() => { _emailService.SendEmail(email, model); }).Start();
					}

					return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = "Lead created succesfully!" });
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { status = "error", message = "Lead already exists!" });
				}

			}
			return Request.CreateResponse(HttpStatusCode.OK, new { status = "error", message = "Lead information is invalid!" });
		}
	}
}