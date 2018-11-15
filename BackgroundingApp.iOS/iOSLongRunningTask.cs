using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundingApp.BackgroundWorkers;
using BackgroundingApp.Constants;
using BackgroundingApp.MessagingSenders;
using UIKit;
using Xamarin.Forms;

namespace BackgroundingApp.iOS
{
    public class iOSLongRunningTask
    {
        nint _taskId;
        CancellationTokenSource _cts;
        private readonly string taskName = "LongRunningTask";

        public iOSLongRunningTask()
        {

        }

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask(taskName, OnExpiration);

            try
            {
                var counter = new TaskCounter();
                await counter.RunCounter(_cts.Token);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    var message = new CancelledMessage();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send(message, MessagingNames.CancelledMessage);
                    });
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        private void OnExpiration()
        {
            _cts.Cancel();
        }


        public void End()
        {
            _cts.Cancel();
        }
    }
}
