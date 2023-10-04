using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Infrastructure.Interface
{
    public interface IDocumentUploadService
    {
        Task<List<UploadResult>> UploadFilesAsync(List<IFormFile> files);
        Task<UploadResult> PhotoUploadAsync(IFormFile pictureFile);
        Task<UploadResult> DocumentUploadAsync(IFormFile documentFile);

    }
}
