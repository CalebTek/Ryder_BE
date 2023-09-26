using Ryder.Application.Common.Hubs;

namespace Ryder.Api.Configurations
{
	public static class SignalRConfiguration
	{
		public static void ConfigureSignalR(this IApplicationBuilder app)
		{
		
			app.UseEndpoints(enpoints =>
			{
				enpoints.MapHub<NotificationHub>("/notification");
				enpoints.MapControllers();

			});

		}
	}
}
