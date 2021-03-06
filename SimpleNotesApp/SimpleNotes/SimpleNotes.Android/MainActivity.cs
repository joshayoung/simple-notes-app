using Android.App;
using Android.Content.PM;
using Android.OS;
using Sentry;

namespace SimpleNotes.Android
{
    [Activity(
        Label = "SimpleNotes", 
        Icon = "@mipmap/icon", 
        RoundIcon = "@mipmap/icon_round", 
        Theme = "@style/MyTheme.Splash", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SentryXamarin.Init(options =>
            {
                options.Dsn = "";
                options.Debug = true;
                options.TracesSampleRate = 1.0;
                options.AddXamarinFormsIntegration();
                // Caching was causing an error, disabline for now:
                options.DisableOfflineCaching();
            });
            
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}