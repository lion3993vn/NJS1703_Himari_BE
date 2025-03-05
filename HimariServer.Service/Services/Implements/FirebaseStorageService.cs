using Firebase.Storage;
using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly FirebaseStorageSettings _firebaseStorageSettings;
        public FirebaseStorageService(IOptions<FirebaseStorageSettings> firebaseStorageSettings)
        {
            _firebaseStorageSettings = firebaseStorageSettings.Value;
        }

        public async Task<BaseResponseModel> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new BaseResponseModel
                {
                    StatusCode = 400,
                    Message = MessageConstants.NO_FILE_UPLOAD,
                    Data = null
                };
            }

            // Check file extension
            var fileExtension = Path.GetExtension(file.FileName).ToLower().Trim();
            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = MessageConstants.IMAGE_EXTENSION_NOT_SUPPORT,
                    Data = null
                };
            }

            // Generate a unique file name
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            // Create Firebase Storage instance
            var storage = new FirebaseStorage(_firebaseStorageSettings.BucketName);

            // Upload the file to Firebase Storage
            using (var stream = file.OpenReadStream())
            {
                var uploadTask = await storage
                    .Child("Himari")
                    .Child(fileName)
                    .PutAsync(stream);

                // Return success with the download URL
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.UPLOAD_FILE_SUCCESS,
                    Data = uploadTask
                };
            }
        }

    }
}
