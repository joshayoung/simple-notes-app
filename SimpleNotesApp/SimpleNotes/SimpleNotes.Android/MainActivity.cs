using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Sentry;
using Shared;

namespace SimpleNotes.Android
{
    [Activity(
        Label = "SimpleNotes", 
        Icon = "@mipmap/icon", 
        RoundIcon = "@mipmap/icon_round", 
        Theme = "@style/NoteTheme", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            ProjectEnvironmentalVariables.Load(dotenv);

            var dsn = System.Environment.GetEnvironmentVariable("DSN") ?? string.Empty;
            
            SentryXamarin.Init(options =>
            {
                options.Dsn = dsn;
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