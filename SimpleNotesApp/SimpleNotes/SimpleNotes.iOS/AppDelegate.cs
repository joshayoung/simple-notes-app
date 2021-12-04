using System;
using System.IO;
using Foundation;
using Sentry;
using Shared;
using UIKit;

namespace SimpleNotes.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary opt)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            ProjectEnvironmentalVariables.Load(dotenv);

            var dsn = Environment.GetEnvironmentVariable("DSN") ?? string.Empty;
            
            SentryXamarin.Init(options =>
            {
                options.Dsn = "";
                options.Debug = true;
                options.TracesSampleRate = 1.0;
                options.AddXamarinFormsIntegration();
                // Caching was causing an error, disabline for now:
                options.DisableOfflineCaching();
            });
            
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, opt);
        }
    }
}