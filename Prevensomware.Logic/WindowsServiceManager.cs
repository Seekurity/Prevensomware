using System;
using System.ServiceProcess;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class WindowsServiceManager
    {
        public Action<string, LogType> LogDelegate { get; set; }
        public ServiceState GetServiceState(DtoServiceInfo serviceInfo)
        {
            var serviceController = new ServiceController(serviceInfo.Name);
            try
            {
                return serviceController.Status == ServiceControllerStatus.Running
                    ? ServiceState.Running
                    : ServiceState.Stopped;
            }
            catch
            {
                LogDelegate("Can't access Prevensomware Scheduler.",LogType.Error);
                return ServiceState.Stopped;
            }
        }

        public bool StartService(DtoServiceInfo serviceInfo)
        {
            var serviceController = new ServiceController(serviceInfo.Name);
            try
            {
                if (serviceController.Status.Equals(ServiceControllerStatus.Running) || serviceController.Status.Equals(ServiceControllerStatus.StartPending))
                {
                    serviceController.Stop();
                    LogDelegate?.Invoke("Stopping Prevensomware Scheduler.",LogType.Info);
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                LogDelegate?.Invoke("Starting Prevensomware Scheduler.", LogType.Info);
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
                ServiceHelper.ChangeStartMode(serviceController, ServiceStartMode.Automatic);
                LogDelegate?.Invoke("Prevensomware Scheduler started succesfully.", LogType.Success);
                return true;
            }
            catch(Exception e)
            {
                LogDelegate?.Invoke("Couldn't start Prevensomware Scheduler.", LogType.Error);
                return false;
            }
        }
    }
}
