using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace TechWizWebApp.Services
{
    public interface IFileService
    {
        public Task<string> UploadImage(IFormFile image);
        public Task UploadImageWithFirebase(FileStream fileStream, string filename);

    }
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _env = hostingEnvironment;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            // Kiem tra neu folder da duoc tao chua
            var folderImagePath = CreateFolderIfNotExist();

            // Kiem tra phan mo rong theo dieu kien
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!CheckExtension(extension))
            {
                return "Invalid file extension.";
            }

            // Tao filename theo Guid de tranh trung lap
            var fileName = GetRandomFilename(extension);
            var filePath = Path.Combine(folderImagePath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
               // await Task.Run(() => UploadImageWithFirebase(stream,fileName));
                image.CopyTo(stream);
            }

            return filePath;
        }


        public async Task UploadImageWithFirebase(FileStream fileStream, string filename)
        {
            string ApiKey = "AIzaSyBhlhuPkt4edumjWfbU1rDjHzjlspGkj6c";
            string Bucket = "techwizwebapp.appspot.com";
            string AuthEmail = "techwiz5@gmail.com";
            string AuthPassword = "123456";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Bucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("Images")
                .Child(filename)
                .PutAsync(fileStream, cancellation.Token);

            try
            {
                string link = await task;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception was thrown: {0}", e);
                throw;
            }

            throw new NotImplementedException();
        }




        // private 
        private string CreateFolderIfNotExist()
        {
            var folderPath = Path.Combine(_env.ContentRootPath, "Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        private bool CheckExtension(string fileExtension)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(fileExtension))
                return false;

            return true;
        }

        private string GetRandomFilename(string extension)
        {
            string randomFileName = Guid.NewGuid().ToString() + extension;
            return randomFileName;
        }


    }

}
