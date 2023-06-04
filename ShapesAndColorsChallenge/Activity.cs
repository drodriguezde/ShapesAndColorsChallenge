using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
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
            SetFullScreen();
            SetContentView(_view);
            _game.Run();
        }

        /// <summary>
        /// Solucionado el obsoleto.
        /// </summary>
        void SetFullScreen()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                var windowInsetsController = Window.DecorView.WindowInsetsController;
                Window.SetDecorFitsSystemWindows(false);
                windowInsetsController.Hide(WindowInsets.Type.NavigationBars());
                windowInsetsController.Hide(WindowInsets.Type.StatusBars());
            }
            else
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.Fullscreen | SystemUiFlags.HideNavigation);/*Quita los botones de navegación del sistema*/
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
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
            SetFullScreen();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            SetFullScreen();
        }
    }
}
