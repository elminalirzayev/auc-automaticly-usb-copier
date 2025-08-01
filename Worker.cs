using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace AUC
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ManagementEventWatcher _watcher;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("USB track service started.");

            var query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_LogicalDisk'");
            _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += UsbInserted;
            _watcher.Start();

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("USB track service stopped.");
            _watcher.Stop();
            _watcher.Dispose();
            return base.StopAsync(cancellationToken);
        }

        private void UsbInserted(object sender, EventArrivedEventArgs e)
        {
            try
            {
                var disk = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string driveLetter = disk["DeviceID"].ToString();

                DriveInfo drive = new DriveInfo(driveLetter);
                if (drive.DriveType == DriveType.Removable)
                {
                    _logger.LogInformation($"USB inserted: {driveLetter}");
                    CopyUsbContent(driveLetter);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
        }

        private void CopyUsbContent(string sourceDrive)
        {
            string destinationRoot = @"C:\auc-backup";
            string destinationPath = Path.Combine(destinationRoot, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            try
            {
                Directory.CreateDirectory(destinationPath);

                foreach (string dirPath in Directory.GetDirectories(sourceDrive, "*", SearchOption.AllDirectories))
                {
                    string targetDir = dirPath.Replace(sourceDrive, destinationPath);
                    Directory.CreateDirectory(targetDir);
                }

                foreach (string filePath in Directory.GetFiles(sourceDrive, "*.*", SearchOption.AllDirectories))
                {
                    string destFile = filePath.Replace(sourceDrive, destinationPath);
                    File.Copy(filePath, destFile, true);
                }

                _logger.LogInformation($"Copieng completed: {destinationPath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Copieng error: {ex.Message}");
            }
        }
    }
}