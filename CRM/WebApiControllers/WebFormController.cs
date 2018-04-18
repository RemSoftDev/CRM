using AutoMapper;
using CRM.Models;
using CRMData.Contexts;
using CRMData.Entities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CRM.WebApiControllers
{
    public class WebFormController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateLead(LeadViewModel model)
        {
            if (ModelState.IsValid)
            {
                Lead lead = Mapper.Map<LeadViewModel, Lead>(model);
                using (BaseContext context = new BaseContext())
                {
                    if (context.Leads.FirstOrDefault(l => l.Email == lead.Email) == null)
                    {
                        context.Leads.Add(lead);
                        context.SaveChanges();

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