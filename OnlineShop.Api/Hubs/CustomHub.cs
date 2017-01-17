using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace OnlineShop.Api.Hubs
{
    public class CustomHub : Hub
    {
        public static void UpdateProductCosts()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<CustomHub>();
            context.Clients.All.updateCosts();
        }
    }
}