using GitMonitor.Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GitMonitor.DomainModel.DTO;
using GitMonitor.DomainModel;
using System;

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
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public HttpResponseMessage AddErrorLog(ErrorLog errorLog)
        {
            try
            {
                ErrorRepo.AddErrorLog(errorLog);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}