using AutoMapper;
using CRM.Models;
using CRM.Services;
using CRM.DAL.Contexts;
using CRM.DAL.Entities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CRM.DAL.Repository;

namespace CRM.WebApiControllers
{
    public class WebFormController : ApiController
    {
	    private readonly IUnitOfWork _unitOfWork;

	    public WebFormController(IUnitOfWork unitOfWork)
	    {
		    _unitOfWork = unitOfWork;
	    }

        [HttpPost]
        public HttpResponseMessage CreateLead(LeadViewModel model)
        {
            var phone = model.Phones.FirstOrDefault();
            phone.Type = Enums.PhoneType.HomePhone;

            if (ModelState.IsValid)
            {
                Lead lead = Mapper.Map<LeadViewModel, Lead>(model);
                lead.IsConverted = false;

                if (_unitOfWork.LeadsRepository.FindBy(l => l.Email == lead.Email) == null)
                {
	                _unitOfWork.LeadsRepository.Create(lead);
	                _unitOfWork.Save();

                    model.Id = lead.Id;

                    if (!string.IsNullOrEmpty(model.Message))
                    {
                        EmailViewModel email = new EmailViewModel {
                            To = "csharpcrm@gmail.com",
                            Subject = "Lead Registered",
                            Body = model.Message,
                            WasReceived = true
                        };

                        new Task(() => { EmailService.SendEmail(email, model); }).Start();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = "Lead created succesfully!" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "error", message = "Lead already exists!" });
                }
               
            }
            return Request.CreateResponse(HttpStatusCode.OK, new {status = "error", message = "Lead information is invalid!" });
        }
    }
}