using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.FileModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/firebase")]
    [ApiController]
    public class FirebaseController : BaseController
    {
        private readonly IFirebaseStorageService _firebaseStorageService;

        public FirebaseController(IFirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        /// <summary>
        /// Upload an image file to Firebase Storage
        /// </summary>
        /// <param name="file">The image file (JPG, JPEG, PNG)</param>
        /// <returns>URL of the uploaded image</returns>
        [Authorize(Roles = "3,4")]
        [HttpPost("upload")]
        public Task<IActionResult> UploadImage(IFormFile file)
        {
            return ValidateAndExecute(async () => await _firebaseStorageService.UploadImageAsync(file));
        }

    }
}
