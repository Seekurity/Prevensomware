using System;
using System.ServiceProcess;
using Prevensomware.Dto;

namespace Prevensomware.Logic
{
    public class WindowsServiceManager
    {
        public Action<string, LogType> LogDelegate { get; set; }
        private ServiceController _serviceController;

        public WindowsServiceManager()
        {
            _serviceController = new ServiceController();
        }
        public ServiceState GetServiceState(DtoServiceInfo serviceInfo)
        {
            _serviceController.DisplayName = serviceInfo.Name;
            _serviceController.ServiceName = serviceInfo.Name;
            try
            {
                return _serviceController.Status == ServiceControllerStatus.Running
                    ? ServiceState.Running
                    : ServiceState.Stopped;
            }
            catch
            {
                LogDelegate("Can't access Scheduler.",LogType.Error);
                return ServiceState.Stopped;
            }
        }
        public bool StopService(DtoServiceInfo serviceInfo)
        {
            _serviceController.DisplayName = serviceInfo.Name;
            _serviceController.ServiceName = serviceInfo.Name;
            try
            {
                _serviceController.Stop();
                LogDelegate?.Invoke("Stopping Scheduler.", LogType.Info);
                _serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                LogDelegate?.Invoke("Scheduler stopped successfully.", LogType.Success);
                return true;
            }
            catch (Exception e)
            {
                LogDelegate?.Invoke("Couldn't stop Scheduler.", LogType.Error);
                return false;
            }
        }
        public bool StartService(DtoServiceInfo serviceInfo)
        {
            _serviceController.DisplayName = serviceInfo.Name;
            _serviceController.ServiceName = serviceInfo.Name;
            try
            {
                if (_serviceController.Status.Equals(ServiceControllerStatus.Running) || _serviceController.Status.Equals(ServiceControllerStatus.StartPending))
                {
                    _serviceController.Stop();
                    LogDelegate?.Invoke("Stopping Scheduler.",LogType.Info);
                    _serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                LogDelegate?.Invoke("Starting Scheduler.", LogType.Info);
                _serviceController.Start();
                _serviceController.WaitForStatus(ServiceControllerStatus.Running);
                ServiceHelper.ChangeStartMode(_serviceController, ServiceStartMode.Automatic);
                LogDelegate?.Invoke($"Scheduler started succesfully with {serviceInfo.UserSettings.SelectedFileExtensionList.Count} Extensions and {serviceInfo.Interval} Hrs Interval.", LogType.Success);
                return true;
            }
            catch(Exception e)
            {
                LogDelegate?.Invoke("Couldn't start Scheduler.", LogType.Error);
                return false;
            }
        }
    }
}
