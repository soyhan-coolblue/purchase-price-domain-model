using System;
using System.Linq;
using System.Threading.Tasks;

using PurchasePrices.Domain.Entities;
using PurchasePrices.Domain.Ports;

namespace PurchasePrices.Domain.Usecases
{
    public class SavePurchasePrice
    {
        private readonly IPurchasePriceRepository _purchasePriceRepository;

        public SavePurchasePrice(IPurchasePriceRepository purchasePriceRepository)
        {
            _purchasePriceRepository = purchasePriceRepository;
        }

        public async Task Invoke(PurchasePrice purchasePrice)
        {
            var existingPrices
                = await _purchasePriceRepository.GetFor(purchasePrice.SupplierId, purchasePrice.ProductId);

            var existingUpcomingTemporaryPrice =
                existingPrices.FirstOrDefault(a => a.IsUpcoming && a.PriceType == PriceType.Temporary);

            if (existingUpcomingTemporaryPrice != null)
            {
                await _purchasePriceRepository.Delete(existingUpcomingTemporaryPrice);
            }

            await _purchasePriceRepository.Save(purchasePrice);
        }
    }
}