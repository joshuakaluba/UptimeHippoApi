﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using UptimeHippoApi.Data.Models.Domain.Entity;
using UptimeHippoApi.Data.Models.Domain.ViewModels;
using UptimeHippoApi.UptimeHandler.Services.Monitoring;

namespace UptimeHippoApi.Web.Controllers
{
    public sealed class MonitorsController : BaseController<MonitorsController>
    {
        private readonly IMonitorsRepository _monitorsRepository;
        private readonly IMonitorLogsRepository _monitorLogsRepository;
        private readonly IMonitoringService _monitoringService;

        public MonitorsController(UserManager<ApplicationUser> userManager,
            IMonitorsRepository monitorsRepository,
            IMonitoringService monitoringService,
            IMonitorLogsRepository monitorLogsRepository,
            ILogger<MonitorsController> logger)
            : base(userManager, logger)
        {
            _monitorsRepository = monitorsRepository;
            _monitoringService = monitoringService;
            _monitorLogsRepository = monitorLogsRepository;
        }

        //Monitors/GetMonitors
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetMonitors()
        {
            try
            {
                var user = await GetCurrentAuthenticatedUser();

                var monitors = await _monitorsRepository.GetMonitorsByUser(user);

                var monitorLogs
                    = await _monitoringService.Monitor
                        (monitors, _monitorsRepository, _monitorLogsRepository);

                monitors = await _monitorsRepository.GetMonitorsByUser(user);

                return Ok(monitors);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Monitors/CheckMonitorsStatus
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CheckMonitorsStatus()
        {
            try
            {
                var user = await GetCurrentAuthenticatedUser();

                var monitors = await _monitorsRepository.GetMonitorsByUser(user);

                var monitorLogs
                    = await _monitoringService.Monitor
                        (monitors, _monitorsRepository, _monitorLogsRepository);

                return Ok(monitorLogs);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Monitors/CreateMonitor
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateMonitor([FromBody] MonitorViewModel monitorViewModel)
        {
            try
            {
                var user = await GetCurrentAuthenticatedUser();

                var monitor = new Monitor(monitorViewModel.Url);

                monitor.ApplicationUserId = user.Id;
                monitor.Interval = monitorViewModel.Interval;
                monitor.Type = monitorViewModel.Type;
                monitor.Name = monitorViewModel.Name;
                monitor.KeyWord = monitorViewModel.KeyWord;
                monitor.Port = monitorViewModel.Port;

                await _monitorsRepository.CreateMonitor(monitor);

                return Ok(monitorViewModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Monitors/UpdateMonitor
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> UpdateMonitor(Guid id, [FromBody] MonitorViewModel monitorViewModel)
        {
            try
            {
                var user = await GetCurrentAuthenticatedUser();

                var monitor = await _monitorsRepository.FindMonitor(id);

                if (monitor != null && monitor.ApplicationUserId == user.Id)
                {
                    monitor.Active = monitorViewModel.Active;
                    monitor.Interval = monitorViewModel.Interval;
                    monitor.Type = monitorViewModel.Type;
                    monitor.Url = monitorViewModel.Url;
                    monitor.Name = monitorViewModel.Name;
                    monitor.KeyWord = monitorViewModel.KeyWord;
                    monitor.Port = monitorViewModel.Port;

                    await _monitorsRepository.UpdateMonitor(monitor);
                    return Ok(monitor);
                }

                return BadRequest(new ErrorMessage("Unable to update monitor"));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Monitors/DeleteMonitor
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> DeleteMonitor(Guid id)
        {
            try
            {
                var monitor = await _monitorsRepository.FindMonitor(id);

                var user = await GetCurrentAuthenticatedUser();

                if (monitor != null && monitor.ApplicationUserId == user.Id)
                {
                    await _monitorsRepository.DeleteMonitor(monitor);
                    return Ok(monitor);
                }

                return BadRequest(new ErrorMessage("Unable to delete monitor"));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }
    }
}