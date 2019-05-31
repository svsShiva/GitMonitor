using GitMonitor.Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GitMonitor.DomainModel.DTO;
using System;

namespace GitMonitor.Service.ConsoleApp.Controllers
{
    public class EmailGroupController : ApiController
    {
        EmailGroupRepository _emailGroup;

        public EmailGroupController()
        {
            _emailGroup = new EmailGroupRepository();
        }

        public HttpResponseMessage GetAllEmailGroups()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _emailGroup.GetAllEmailGroups());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage AddEmailGroup(EmailGroup emailGroup)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _emailGroup.Add(emailGroup));
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public HttpResponseMessage DeleteEmailGroup(long id)
        {
            try
            {
                _emailGroup.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateEmailGroup(EmailGroup emailGroup)
        {
            try
            {
                _emailGroup.Update(emailGroup);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
