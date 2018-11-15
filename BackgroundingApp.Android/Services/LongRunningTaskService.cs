using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using BackgroundingApp.BackgroundWorkers;
using BackgroundingApp.Constants;
using BackgroundingApp.MessagingSenders;
using Xamarin.Forms;

namespace BackgroundingApp.Droid.Services
{
    [Service]
    public class LongRunningTaskService : Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
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
            });

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}
