using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HimariServer.Service.BusinessModels.FileModels
{
    public class FileUploadModel
    {
        [Required]
        public IFormFile File { get; set; }
        
        public string FolderName { get; set; } = "images";
    }
}
