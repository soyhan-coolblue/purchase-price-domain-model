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
            var purchasePrices =
                await _purchasePriceRepository.GetFor(purchasePrice.SupplierId, purchasePrice.ProductId);

            var upcomingTemporaryPurchasePrice =
                purchasePrices.SingleOrDefault(a => a.IsUpcoming && a.PriceType == PriceType.Temporary);

            if (upcomingTemporaryPurchasePrice != null)
                await _purchasePriceRepository.Delete(upcomingTemporaryPurchasePrice);

            await _purchasePriceRepository.Save(purchasePrice);
        }
    }
}