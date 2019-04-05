using GitMonitor.Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using GitMonitor.DomainModel.DTO;
using GitMonitor.Service.ConsoleApp.Utilities;
using GitMonitor.DomainModel.ViewModels;

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
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllTrackedReposViewModel());
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
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllUnTrackedReposViewModel());
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
                return Request.CreateResponse(HttpStatusCode.OK, _repo.GetUIRepoByID(id));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutEditFormViewModel(EditFormViewModel repo)
        {
            try
            {
                _repo.UpdateFromUI(repo);

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
                Task.Run(() =>
                {
                    var repo = _repo.GetRepoByID(id);
                    GitUtility.RunTasks(repo, true);
                });
               
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}