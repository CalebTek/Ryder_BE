using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Ryder.Domain.Entities;
using Ryder.Domain.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ryder.Application.Common.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {

		public async Task NotifyRidersOfIncomingRequest(string riderConnectionId)
		{
			await Clients.Client(riderConnectionId).SendAsync("IncomingRequest", "You have an incoming request.");	
			
		}


		public async Task NotifyUserOfRequestAccepted(string userConnectionId)
		{
			await Clients.Client(userConnectionId).SendAsync("RequestAccepted", "Your request has been accepted.");
		}

		public async Task NotifyUserAndRiderOfOrderCompleted(string userConnectionId, string riderConnectionId)
		{
			await Clients.Client(userConnectionId).SendAsync("OrderCompleted", "Your order has been completed.");
			await Clients.Client(riderConnectionId).SendAsync("OrderCompleted", "The order has been completed.");
		}


	}
}
