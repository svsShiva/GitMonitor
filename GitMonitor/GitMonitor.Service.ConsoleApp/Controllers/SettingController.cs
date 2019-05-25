using GitMonitor.Repository;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GitMonitor.DomainModel.DTO;
using System;
using System.Collections.Generic;

namespace GitMonitor.Service.ConsoleApp.Controllers
{
    public class SettingController : ApiController
    {
        SettingsRepository _settingsRepository;

        public SettingController()
        {
            _settingsRepository = new SettingsRepository();
        }

        public HttpResponseMessage GetAllSetting()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _settingsRepository.GetAllSettings());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateSetting(List<Setting> settings)
        {
            try
            {
                _settingsRepository.Update(settings);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
