using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackgroundingApp.BackgroundWorkers;
using BackgroundingApp.Constants;
using BackgroundingApp.MessagingSenders;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace BackgroundingApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        iOSLongRunningTask longRunningTask;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            SubcribeToMessages();

            return base.FinishedLaunching(app, options);
        }

        private void SubcribeToMessages()
        {
            MessagingCenter.Subscribe<LongRunningTask>(this, MessagingNames.StartLongRunningTask, async (arg) =>
            {
                longRunningTask = new iOSLongRunningTask();
                await longRunningTask.Start();
            });

            MessagingCenter.Subscribe<LongRunningTask>(this, MessagingNames.EndLongRunningTask, (arg) =>
            {
                 longRunningTask.End();
            });
        }
    }
}
