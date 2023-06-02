using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class;

namespace ShapesAndColorsChallenge
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleTask,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.Keyboard |
        ConfigChanges.KeyboardHidden |
        ConfigChanges.ScreenSize |
        //ConfigChanges.ColorMode | 
        //ConfigChanges.Density | 
        //ConfigChanges.FontScale | 
        //ConfigChanges.LayoutDirection | 
        //ConfigChanges.Navigation |
        ConfigChanges.ScreenLayout |
        ConfigChanges.ScreenSize |
        //ConfigChanges.SmallestScreenSize |
        //ConfigChanges.Touchscreen |        
        //ConfigChanges.Locale |
        //ConfigChanges.Mcc |
        //ConfigChanges.Mnc |
        ConfigChanges.UiMode
    )]
    public class Activity : AndroidGameActivity
    {
        private Main _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _game = new Main();
            _view = _game.Services.GetService(typeof(View)) as View;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.Fullscreen | SystemUiFlags.HideNavigation);/*Quita los botones de navegación del sistema*/

            SetContentView(_view);
            _game.Run();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }
    }
}
