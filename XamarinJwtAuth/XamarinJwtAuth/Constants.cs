using System;
namespace XamarinJwtAuth
{
    public static class Constants
    {
        public static string AuthorizeUri => "https://localhost:5001/connect/authorize";
        public static string ClientId => "xamarinWebAuth";
        public static string Scope = "";
        public static string RedirectUri = "xamarinWebAuth://";
    }
}
