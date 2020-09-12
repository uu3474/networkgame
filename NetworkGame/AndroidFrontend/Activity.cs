using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace AndroidFrontend
{
    [Activity(Label = "Network"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        /*, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize*/)]
    public class Activity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        AGame m_game;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //var glVersion = ((ActivityManager)GetSystemService(Context.ActivityService)).DeviceConfigurationInfo.ReqGlEsVersion;

            m_game = new AGame();
            m_game.Run();

            var view = (SurfaceView)m_game.Services.GetService(typeof(View));

            //Black screen blink fix(https://stackoverflow.com/questions/5391089/how-to-make-surfaceview-transparent)
            view.SetZOrderOnTop(true);
            view.Holder.SetFormat(Android.Graphics.Format.Transparent);
            //=========================================================

            SetContentView(view);
        }

        protected override void OnStop()
        {
            m_game?.SaveDataIfNeed();
            base.OnStop();
        }
    }
}

