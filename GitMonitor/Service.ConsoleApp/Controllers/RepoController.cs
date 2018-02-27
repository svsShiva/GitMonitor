using Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.ConsoleApp.Controllers
{
    public class RepoController : ApiController
    {
        RepoRepository _repo;

        public RepoController()
        {
            _repo = new RepoRepository();
        }

        public HttpResponseMessage GetAllActiveRepos()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllActiveRepos());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}