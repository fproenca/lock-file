using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace lock_file
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;
        private readonly FileSystemWatcher _fs;
        private readonly System.Timers.Timer _timer;
        private readonly File _file;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _file = new File();
            _fs = new FileSystemWatcher(@"C:\Users\francisco\Desktop\Origem");
            _timer = new System.Timers.Timer(30000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start StartAsync");
            _fs.Created += _fs_Created;
            _fs.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() => _file.CopyEventually(@"C:\Users\francisco\Desktop\Origem", @"C:\Users\francisco\Desktop\Destino"));
        }

        private void _fs_Created(object sender, FileSystemEventArgs e)
        {
            Task.Run(() =>
                _file.Copy(e.Name, @"C:\Users\francisco\Desktop\Origem", @"C:\Users\francisco\Desktop\Destino"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start StopAsync");
            return Task.CompletedTask;
        }

    }
}
