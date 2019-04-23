using GitMonitor.Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using GitMonitor.DomainModel.DTO;
using GitMonitor.Service.ConsoleApp.Utilities;

namespace GitMonitor.Service.ConsoleApp.Controllers
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
        public HttpResponseMessage PutEditFormViewModel(Repo repo)
        {
            try
            {
                //TODO check the method and implement
               // _repo.UpdateFromUI(repo);

                Task.Run(() =>
                {
                    Repo obj = _repo.GetRepoByID(repo.RepoID);
                    GitUtility.RunTasks(obj, false);
                });

                return Request.CreateResponse(HttpStatusCode.OK, repo.RepoID);
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

                Task.Run(() =>
                {
                    GitUtility.RunTasks(repo, true);
                });
               
                return Request.CreateResponse(HttpStatusCode.OK, repo);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}