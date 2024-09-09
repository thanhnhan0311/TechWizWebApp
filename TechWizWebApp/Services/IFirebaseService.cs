using Firebase.Auth;

namespace TechWizWebApp.Services
{
    public interface IFirebaseService
    {
        Task Upload(FileStream fileStream,string filename);
    }
    public class FirebaseService : IFirebaseService
    {
        private static string ApiKey = "AIzaSyBhlhuPkt4edumjWfbU1rDjHzjlspGkj6c";
        private static string Bucket = "techwizwebapp.appspot.com";
        private static string AuthEmail = "techwiz5@gmail.com";
        private static string AuthPassword = "123456";

        public Task Upload(FileStream fileStream, string filename)
        {
            

            throw new NotImplementedException();
        }
    }
}
