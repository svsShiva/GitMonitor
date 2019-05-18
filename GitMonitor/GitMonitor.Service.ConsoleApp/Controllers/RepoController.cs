using GitMonitor.Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using GitMonitor.DomainModel.DTO;
using GitMonitor.Service.ConsoleApp.Utilities;
using System;

namespace GitMonitor.Service.ConsoleApp.Controllers
{
    public class RepoController : ApiController
    {
        RepoRepository _repo;
        ErrorRepo _errorRepo;

        public RepoController()
        {
            _repo = new RepoRepository();
            _errorRepo = new ErrorRepo();
        }

        public HttpResponseMessage GetAllTrackedRepos()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllTrackedRepos());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public HttpResponseMessage GetAllUnTrackedRepos()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllUnTrackedRepos());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public HttpResponseMessage GetRepoByID(long id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetRepoByID(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateRepo(Repo repo)
        {
            try
            {
                _repo.Update(repo);

                Task.Run(() =>
                {
                    GitUtility.RunTasks(repo, false);
                });

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public HttpResponseMessage StopTrackingRepo(long id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _repo.StopTrackingRepo(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage RefreshRepo(long id)
        {
            try
            {
                var repo = _repo.GetRepoByID(id);
                GitUtility.RunTasks(repo, true);

                return Request.CreateResponse(HttpStatusCode.OK, repo);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}