using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Payroll_Manager.Hubs
{

  public class AuctionHub : Hub
  {

    public async Task SendAsync(int productId, int luotdat)
    {
      await Clients.All.SendAsync(productId.ToString(), luotdat);
    }
  }
}
