using AutoMapper;
using CRM.Models;
using CRM.Services;
using CRMData.Contexts;
using CRMData.Entities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRM.WebApiControllers
{
    public class WebFormController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateLead(LeadViewModel model)
        {
            var phone = model.Phones.FirstOrDefault();
            phone.Type = Enums.PhoneType.HomePhone;

            if (ModelState.IsValid)
            {
                Lead lead = Mapper.Map<LeadViewModel, Lead>(model);
                lead.IsConverted = false;
                using (BaseContext context = new BaseContext())
                {
                    if (context.Leads.FirstOrDefault(l => l.Email == lead.Email) == null)
                    {
                        context.Leads.Add(lead);
                        context.SaveChanges();

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
               
            }
            return Request.CreateResponse(HttpStatusCode.OK, new {status = "error", message = "Lead information is invalid!" });
        }
    }
}