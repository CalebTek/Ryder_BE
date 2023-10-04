using AspNetCoreHero.Results;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ryder.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ryder.Infrastructure.Implementation
{
    public class DocumentUploadService : IDocumentUploadService
    {
        private readonly Cloudinary _cloudinary;
   
        public DocumentUploadService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<List<UploadResult>> UploadFilesAsync(List<IFormFile> files)
        {
            var uploadResult = new List<UploadResult>();
            foreach (var file in files)
            {
                // Handle invalid input
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Invalid file data.");
                }

                const int pictureSize = 1000 * 1024;
                if (file.Length > pictureSize)
                {
                    throw new ArgumentException("File size exceed the maximum limit (1mb).");
                }

                var allowedExtensions = new[] { ".jpg", ".png", ".pdf" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException( "Only jpg and png files are allowed.");
                }

                //save files to cloudinary
                using (var documentStream = file.OpenReadStream())
                {
                    string filename = Guid.NewGuid().ToString() + file.FileName;
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(filename + Guid.NewGuid().ToString(), documentStream),
                        PublicId = "Rider Document/" + filename
                    };
                    var result = await _cloudinary.UploadAsync(uploadParams);
                    uploadResult.Add(result);
                }
            }
            return uploadResult;
        }

        public async Task<UploadResult> DocumentUploadAsync(IFormFile documentFile)
        {
            // Handle invalid input
            if (documentFile == null || documentFile.Length == 0)
            {
                throw new ArgumentException("Invalid file data.");
            }

            const int pictureSize = 1000 * 1024;
            if (documentFile.Length > pictureSize)
            {
                throw new ArgumentException("File size exceed the maximum limit (1mb).");
            }

            var allowedExtensions = new[] { ".jpg", ".png", ".pdf"};
            var fileExtension = Path.GetExtension(documentFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Only jpg and png files are allowed.");
            }

            //save image to cloudinary
            var uploadResult = new ImageUploadResult();
            using (var documentStream = documentFile.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + documentFile.FileName;
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(filename + Guid.NewGuid().ToString(), documentStream),
                    PublicId = "Rider Document/" + filename
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
                
            }
            return uploadResult;
        }

        public async Task<UploadResult> PhotoUploadAsync(IFormFile photoFile)
        {
            // Handle invalid input
            if (photoFile == null || photoFile.Length == 0)
            {
                throw new ArgumentException("Invalid file data.");
            }

            const int pictureSize = 1000 * 1024;
            if (photoFile.Length > pictureSize)
            {
                throw new ArgumentException("File size exceed the maximum limit (1mb).");
            }

            var allowedExtensions = new[] { ".jpg", ".png", ".pdf" };
            var fileExtension = Path.GetExtension(photoFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Only jpg and png files are allowed.");
            }

            //save image to cloudinary
            var uploadResult = new ImageUploadResult();
            using (var photoStream = photoFile.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + photoFile.FileName;
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(filename + Guid.NewGuid().ToString(), photoStream),
                    PublicId = "Rider Document/" + filename
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            return uploadResult;
        }
    }
}
