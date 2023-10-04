using CloudinaryDotNet;
using System.Security.Principal;

namespace Ryder.Api.Configurations
{
    public static class CloudinaryConfiguration
    {
        public static void ConfigureCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinarySettings = configuration.GetSection("Cloudinary");

            var account = new Account(
                cloudinarySettings["CloudName"],
                cloudinarySettings["ApiKey"],
                cloudinarySettings["ApiSecret"]);

            var cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }
    }
}
