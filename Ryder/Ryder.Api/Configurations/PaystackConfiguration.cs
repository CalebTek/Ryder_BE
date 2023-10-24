using Ryder.Infrastructure.Interface;
using PayStack.Net;
using Ryder.Infrastructure.Common.Extensions;
using Ryder.Infrastructure.Implementation;

namespace Ryder.Api.Configurations
{
    public static class PaystackConfiguration
    {
        public static void ConfigurePaystack(this IServiceCollection services, IConfiguration configuration)
        {
            var paystackSettings = new PaystackSettings();
            configuration.GetSection("Paystack").Bind(paystackSettings);
            services.AddSingleton(paystackSettings);

            string paystackSecretKey = paystackSettings.TestSecretKey;


            //services.AddSingleton<IPaystackService>(provider =>
            //{
            //    var paystack = new PayStackApi(paystackSecretKey);
            //    return new PaystackService(paystack);
            //});


            // Register IPayStackApi with its implementation PayStackApi
            services.AddSingleton<IPayStackApi>(provider =>
            {
                return new PayStackApi(paystackSecretKey);
            });

            services.AddSingleton<IPaystackService, PaystackService>();
        }
    }
}
