using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using BackgroundingApp.MessagingSenders;
using BackgroundingApp.Constants;
using Android.Content;
using BackgroundingApp.Droid.Services;

namespace BackgroundingApp.Droid
{
    [Activity(Label = "BackgroundingApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            SubcribeToMessages();
            LoadApplication(new App());
        }

        private void SubcribeToMessages()
        {
            MessagingCenter.Subscribe<LongRunningTask>(this, MessagingNames.StartLongRunningTask, async (arg) =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<LongRunningTask>(this, MessagingNames.EndLongRunningTask, (arg) =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
        }
    }
}