using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgroundingApp.BackgroundWorkers;
using BackgroundingApp.Constants;
using BackgroundingApp.MessagingSenders;
using Xamarin.Forms;

namespace BackgroundingApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<TickedMessage>(this, MessagingNames.TickedMessage, message =>
            {
                lblStatus.Text = message.Message;
                System.Diagnostics.Debug.WriteLine("Ticked: " + message.Message);
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, MessagingNames.CancelledMessage, message =>
            {
                lblStatus.Text = "Cancelled";
            });
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var message = new LongRunningTask();
            MessagingCenter.Send(message, MessagingNames.StartLongRunningTask);
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            var message = new LongRunningTask();
            MessagingCenter.Send(message, MessagingNames.EndLongRunningTask);
        }
    }
}
