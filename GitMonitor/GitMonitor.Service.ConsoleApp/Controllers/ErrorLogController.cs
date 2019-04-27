using GitMonitor.Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GitMonitor.DomainModel.DTO;
using GitMonitor.DomainModel;

namespace GitMonitor.Service.ConsoleApp.Controllers
{
    public class ErrorLogController : ApiController
    {
        ErrorRepo _errorRepo;

        public ErrorLogController()
        {
            _errorRepo = new ErrorRepo();
        }

        public HttpResponseMessage GetErrorLog()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _errorRepo.GetErrorLog());
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, StringUtility._unexpectedError);
            }
        }

        public HttpResponseMessage AddErrorLog(ErrorLog errorLog)
        {
            try
            {
                ErrorRepo.AddErrorLog(errorLog);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, StringUtility._unexpectedError);
            }
        }
    }
}