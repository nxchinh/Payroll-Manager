using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Helpers;
using Payroll_Manager.Hubs;

namespace Payroll_Manager.Services
{
    public class AuctionService
  {
      private readonly IHubContext<AuctionHub> _auctionHub;
      private readonly UnitOfWork _unitOfWork;
      private readonly IInventoryService _inventoryService;

        public AuctionService(IHubContext<AuctionHub> auctionHub, IInventoryService inventoryService, UnitOfWork unitOfWork)
      {
          _auctionHub = auctionHub;
          _unitOfWork = unitOfWork;
          _inventoryService = inventoryService;
      }
    public async Task StartSaleAsync(int productId, decimal price)
    {
      await _auctionHub.Clients.All.SendAsync(productId.ToString(),price); 
    }
    public async Task<decimal> Buy(int productId)
    {
        Inventory product = _inventoryService.GetById(productId);
        product.LuotDat += 1;
        using (var dbContextTransaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                await _inventoryService.UpdateAsync(product);
                await _unitOfWork.SaveAsync();
                dbContextTransaction.Commit();
            }
            catch (Exception e)
            {
                dbContextTransaction.Rollback();
                throw new Exception("DB Transaction Failed. Rollback Changes. " + e.Message);
            }
        }

        var a = product.LuotDat;
        await _auctionHub.Clients.All.SendAsync(productId.ToString(), product.LuotDat);
        return product.LuotDat;
    }
    public async Task StopSale(int productId)
    {
        await _auctionHub.Clients.All.SendAsync(productId.ToString(), 0.00M);   
    }

  }
}
