using System.IO;
using Microsoft.AspNetCore.Http;

namespace PragyoSala.Services.Dtos
{
    public class VideoUploadDto
    {        
        public IFormFile Video { get; set; }
    }
}