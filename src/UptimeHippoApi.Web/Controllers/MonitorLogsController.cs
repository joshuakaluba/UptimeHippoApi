using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UptimeHippoApi.Common.Models;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Web.Controllers
{
    public class MonitorLogsController : BaseController<MonitorsController>
    {
        private readonly IMonitorsRepository _monitorsRepository;
        private readonly IMonitorLogsRepository _monitorLogsRepository;

        public MonitorLogsController(UserManager<ApplicationUser> userManager,
            IMonitorsRepository monitorsRepository,
            IMonitorLogsRepository monitorLogsRepository,
            ILogger<MonitorsController> logger)
            : base(userManager, logger)
        {
            _monitorsRepository = monitorsRepository;
            _monitorLogsRepository = monitorLogsRepository;
        }

        //MonitorLogs/GetMonitorLogs
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetMonitorLogs(Guid id)
        {
            try
            {
                var user = await GetUser();

                var monitor = await _monitorsRepository.FindMonitor(id);

                if (monitor != null && monitor.ApplicationUserId == user.Id)
                {
                    var monitorLogs = await _monitorLogsRepository.GetMonitorLogsByMonitor(monitor);

                    return Ok(monitorLogs);
                }

                return BadRequest(new ErrorMessage("Unable to get monitor logs"));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }
    }
}