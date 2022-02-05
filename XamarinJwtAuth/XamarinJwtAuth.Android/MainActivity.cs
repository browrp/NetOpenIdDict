using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace XamarinJwtAuth.Droid
{
    [Activity(Label = "XamarinJwtAuth", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        /// <summary>
        /// Adding because this blog said we needed it https://www.davidbritch.com/2020/04/authentication-from-xamarinforms-app.html
        /// But upon reading the microsoft docs https://docs.microsoft.com/en-us/xamarin/essentials/web-authenticator?tabs=ios it doesn't show it
        /// So adding this just in case.  We'll set a breakpoint to see if it's called.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            Xamarin.Essentials.Platform.OnResume();
        }
    }
}