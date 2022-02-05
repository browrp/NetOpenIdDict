using System;
using Android.App;
using Android.Content.PM;

namespace XamarinJwtAuth.Droid
{
    //public class WebAuthenticationCallbackActivity
    //{
    //    public WebAuthenticationCallbackActivity()
    //    {
    //    }
    //}


    //[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    //[IntentFilter(new[] { Intent.ActionView },
    //Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    //DataScheme = "io.identitymodel.native",
    //DataHost = "callback")]
    //public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    //{
    //}


    /// <summary>
    /// https://docs.microsoft.com/en-us/xamarin/essentials/web-authenticator?tabs=android
    /// </summary>
    //const string CALLBACK_SCHEME = "myapp";

    //[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    //[IntentFilter(new[] { Android.Content.Intent.ActionView },
    //    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
    //    DataScheme = CALLBACK_SCHEME)]
    //public class WebAuthenticationCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
    //{
    //}

    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
    DataScheme = "https://oauth.pstmn.io/v1/callback")]
    public class WebAuthenticationCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
    {
    }

    


}
