using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UptimeHippoApi.Common.Exception;
using UptimeHippoApi.Common.Utilities;
using UptimeHippoApi.Data.Containers;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.UptimeHandler.Services.Monitoring
{
    public sealed class MonitoringService : IMonitoringService
    {
        private List<Monitor> _failedMonitors;
        private List<Monitor> _processedMonitors;
        private List<MonitorLog> _monitorLogs;

        public MonitoringService()
        {
            _failedMonitors = new List<Monitor>();
            _processedMonitors = new List<Monitor>();
            _monitorLogs = new List<MonitorLog>();
        }

        private async Task HttpMonitor(IEnumerable<Monitor> httpMonitors)
        {
            foreach (var monitor in httpMonitors)
            {
                monitor.LastMonitorDate = DateTime.UtcNow;

                using (var httpClient = new HttpClient())
                {
                    var monitorLog = new MonitorLog
                    {
                        MonitorId = monitor.Id
                    };

                    var responseCode = 0;

                    try
                    {
                        httpClient.BaseAddress = new Uri(monitor.Url);
                        httpClient.DefaultRequestHeaders.Accept.Clear();

                        var response = await httpClient.GetAsync("");

                        responseCode = (int)response.StatusCode;
                        response.EnsureSuccessStatusCode();

                        monitor.LastMonitorSuccess = true;

                        monitorLog.ResponseCode = responseCode;
                        monitorLog.Successful = true;

                        monitor.Triggered = false;
                    }
                    catch (Exception ex)
                    {
                        monitorLog.ResponseCode = responseCode;
                        monitorLog.Successful = false;
                        monitorLog.ExceptionMessage = ex.Message;

                        monitor.LastMonitorSuccess = false;

                        _failedMonitors.Add(new Monitor(monitor));

                        monitor.Triggered = true;
                    }
                    finally
                    {
                        _processedMonitors.Add(monitor);
                        _monitorLogs.Add(monitorLog);
                    }
                }
            }
        }

        private async Task KeyWordMonitor(IEnumerable<Monitor> keyWordMonitors)
        {
            foreach (var monitor in keyWordMonitors)
            {
                monitor.LastMonitorDate = DateTime.UtcNow;

                using (var httpClient = new HttpClient())
                {
                    var monitorLog = new MonitorLog
                    {
                        MonitorId = monitor.Id
                    };

                    var responseCode = 0;

                    try
                    {
                        httpClient.BaseAddress = new Uri(monitor.Url);
                        httpClient.DefaultRequestHeaders.Accept.Clear();

                        var response = await httpClient.GetAsync("");

                        responseCode = (int)response.StatusCode;
                        response.EnsureSuccessStatusCode();

                        using (var content = response.Content)
                        {
                            var html = await content.ReadAsStringAsync();

                            MonitoringHelper.EnsureKeyWordExists(html, monitor.KeyWord);

                            monitor.LastMonitorSuccess = true;

                            monitorLog.ResponseCode = responseCode;
                            monitorLog.Successful = true;
                            monitor.Triggered = false;
                        }
                    }
                    catch (KeyWordNotFoundException)
                    {
                        monitorLog.ResponseCode = responseCode;
                        monitorLog.Successful = false;
                        monitorLog.ExceptionMessage = "Unable to find keyword";

                        monitor.LastMonitorSuccess = false;

                        _failedMonitors.Add(new Monitor(monitor));
                        monitor.Triggered = true;
                    }
                    catch (Exception ex)
                    {
                        monitorLog.ResponseCode = responseCode;
                        monitorLog.Successful = false;
                        monitorLog.ExceptionMessage = ex.Message;

                        monitor.LastMonitorSuccess = false;

                        _failedMonitors.Add(new Monitor(monitor));
                        monitor.Triggered = true;
                    }
                    finally
                    {
                        _processedMonitors.Add(monitor);
                        _monitorLogs.Add(monitorLog);
                    }
                }
            }
        }

        private void PingMonitor(IEnumerable<Monitor> pingMonitors)
        {
            foreach (var monitor in pingMonitors)
            {
                monitor.LastMonitorDate = DateTime.UtcNow;

                var monitorLog = new MonitorLog
                {
                    MonitorId = monitor.Id
                };

                try
                {
                    var successfullPing = MonitoringHelper.PingHost(monitor.Url);

                    if (successfullPing)
                    {
                        monitor.LastMonitorSuccess = true;
                        monitorLog.Successful = true;
                        monitor.Triggered = false;
                    }
                    else
                    {
                        throw new PingException("Unable to ping ");
                    }
                }
                catch (PingException ex)
                {
                    monitorLog.Successful = false;
                    monitorLog.ExceptionMessage = ex.Message;

                    monitor.LastMonitorSuccess = false;

                    _failedMonitors.Add(new Monitor(monitor));
                    monitor.Triggered = true;
                }
                catch (Exception ex)
                {
                    monitorLog.Successful = false;
                    monitorLog.ExceptionMessage = ex.Message;

                    monitor.LastMonitorSuccess = false;

                    _failedMonitors.Add(new Monitor(monitor));
                    monitor.Triggered = true;
                }
                finally
                {
                    _processedMonitors.Add(monitor);
                    _monitorLogs.Add(monitorLog);
                }
            }
        }

        public async Task<List<MonitorLog>> Monitor(IEnumerable<Monitor> sitesToMonitor,
            IMonitorsRepository monitorsRepository,
            IMonitorLogsRepository monitorLogsRepository)
        {
            if (sitesToMonitor == null)
            {
                sitesToMonitor = await monitorsRepository.GetAllActiveMonitors();
            }

            var httpMonitors
                = sitesToMonitor.Where
                    (monitor => monitor.Type == MonitorTypeEnum.HTTP)
                        .ToList();

            var keyWordMonitors
                = sitesToMonitor.Where
                    (monitor => monitor.Type == MonitorTypeEnum.KEYWORD)
                        .ToList();

            var pingMonitors
                = sitesToMonitor.Where
                    (monitor => monitor.Type == MonitorTypeEnum.PING)
                        .ToList();

            //var portMonitors
            //    = sitesToMonitor.Where
            //        (monitor => monitor.Type == MonitorTypeEnum.PORT)
            //            .ToList();

            await HttpMonitor(httpMonitors);

            await KeyWordMonitor(keyWordMonitors);

            //TODO refactor and run this asynchronously..
            PingMonitor(pingMonitors);

            await monitorsRepository.UpdateMonitors(_processedMonitors);

            await monitorLogsRepository.SaveMonitorLogs(_monitorLogs);

            return _monitorLogs;
        }

        public ApplicationUserMonitors GetFailedMonitors()
        {
            var applicationUserMonitors = new ApplicationUserMonitors();

            foreach (var monitor in _failedMonitors)
            {
                if (!monitor.Triggered)
                {
                    applicationUserMonitors.Add(monitor);
                }
            }

            return applicationUserMonitors;
        }
    }
}