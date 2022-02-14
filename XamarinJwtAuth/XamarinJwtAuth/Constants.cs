using System;
namespace XamarinJwtAuth
{
    public static class Constants
    {
        //public static string AuthorizeUri => "https://localhost:5001/connect/authorize";
        //public static string ClientId => "xamarinWebAuth";
        //public static string Scope = "";
        //public static string RedirectUri = "xamarinWebAuth://";


        
        public static string AuthorityUri = "https://demo.identityserver.io";
        public static string AuthorizeUri = "https://localhost:5001/connect/authorize";
        public static string TokenUri = "https://localhost:5001/connect/token";
        public static string RedirectUri = "xamarinWebAuth://";
        //public static string ApiUri = "https://demo.identityserver.io/api/";
        public static string ClientId = "xamarinWebAuth";
        public static string ClientSecret = "";
        public static string Scope = "api openid offline_access";
        

        /*
        public static string AuthorityUri = "https://demo.identityserver.io";
        public static string AuthorizeUri = "https://localhost:5001/connect/authorize";
        public static string TokenUri = "https://localhost:5001/connect/token";
        public static string RedirectUri = "xamarinWebAuth://";
        //public static string ApiUri = "https://demo.identityserver.io/api/";
        public static string ClientId = "postman";
        public static string ClientSecret = "postman-secret";
        public static string Scope = "api";
        */
    }
}
