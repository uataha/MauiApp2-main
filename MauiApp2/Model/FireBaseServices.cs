using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using System.Threading.Tasks;

namespace MauiApp2.Model
{
    public class FireBaseServices
    {
        static string project_id = "mauiapp-61414";
        static string ApiKey = "AIzaSyAtqlJDEItlklh1y8xEn8njR0U7VmBrfRc";
        static string AuthDomain = $"{project_id}.firebaseapp.com";

        static FirebaseAuthConfig config = new FirebaseAuthConfig()
        {
            ApiKey = ApiKey,
            AuthDomain = AuthDomain,
            Providers = new[] { new EmailProvider() }
        };

        private static FirebaseAuthClient _client;

        static FireBaseServices()
        {
            _client = new FirebaseAuthClient(config);
        }

        public static async Task<User> Login(string email, string pass)
        {
            try
            {
                var res = await _client.SignInWithEmailAndPasswordAsync(email, pass);
                return res.User;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<User> Register(string dispname, string email, string pass)
        {
            try
            {
                var res = await _client.CreateUserWithEmailAndPasswordAsync(email, pass, dispname);
                return res.User;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void Logout()
        {
            try
            {
                _client.SignOut();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }
        }
    }
}
