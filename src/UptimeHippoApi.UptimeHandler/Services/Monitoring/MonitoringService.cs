using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.UptimeHandler.Services.Monitoring
{
    public class MonitoringService : IMonitoringService
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

        private async Task HttpMonitor (List<Monitor> httpMonitors)
        {
            foreach(var monitor in httpMonitors)
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

                       
                    }
                    catch (Exception ex)
                    {
                        monitorLog.ResponseCode = responseCode;
                        monitorLog.Successful = false;
                        monitorLog.ExceptionMessage = ex.Message;

                        monitor.LastMonitorSuccess = false;

                        _failedMonitors.Add(monitor);
                    }
                    finally
                    {
                        _processedMonitors.Add(monitor);
                        _monitorLogs.Add(monitorLog);
                    }
                    
                }
            }
        }

        public async Task Monitor(IMonitorsRepository monitorsRepository)
        {
            var allSitesToMonitor = await monitorsRepository.GetAllActiveMonitors();

            var httpMonitors
                = allSitesToMonitor.Where
                    (monitor => monitor.Type == MonitorTypeEnum.HTTP)
                        .ToList();

            await this.HttpMonitor(httpMonitors);

            await monitorsRepository.UpdateMonitors(_processedMonitors);


            //var keyWordMonitors
            //    = allSitesToMonitor.Where
            //        (monitor => monitor.Type == MonitorTypeEnum.KEYWORD)
            //            .ToList();

            //var pingMonitors
            //    = allSitesToMonitor.Where
            //        (monitor => monitor.Type == MonitorTypeEnum.PING)
            //            .ToList();

            //var portMonitors
            //    = allSitesToMonitor.Where
            //        (monitor => monitor.Type == MonitorTypeEnum.PORT)
            //            .ToList();
        }
    }
}