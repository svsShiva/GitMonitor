using GitMonitor.Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GitMonitor.DomainModel.DTO;
using GitMonitor.DomainModel;

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
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, StringUtility._unexpectedError);
            }
        }


        [HttpPost]
        public HttpResponseMessage AddEmailGroup(EmailGroup emailGroup)
        {
            try
            {
                _emailGroup.Add(emailGroup);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, StringUtility._unexpectedError);
            }
        }
        
        public HttpResponseMessage DeleteEmailGroup(long id)
        {
            try
            {
                _emailGroup.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, StringUtility._unexpectedError);
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
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, StringUtility._unexpectedError);
            }
        }
    }
}
