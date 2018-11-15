using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundingApp.Constants;
using BackgroundingApp.MessagingSenders;
using Xamarin.Forms;

namespace BackgroundingApp.BackgroundWorkers
{
    public class TaskCounter
    {
        public TaskCounter()
        {

        }

        public async Task RunCounter(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < long.MaxValue; i++)
                {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(TimeSpan.FromSeconds(1));

                    var message = new TickedMessage()
                    {
                        Message = i.ToString()
                    };

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send(message, MessagingNames.TickedMessage);
                    });
                }
            });
        }
    }
}
