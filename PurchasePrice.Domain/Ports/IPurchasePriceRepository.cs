using System.Collections.Generic;
using System.Threading.Tasks;

using PurchasePrices.Domain.Entities;

namespace PurchasePrices.Domain.Ports
{
    public interface IPurchasePriceRepository
    {
        Task Save(PurchasePrice purchasePrice);
        Task Delete(PurchasePrice purchasePrice);
        Task Update(PurchasePrice purchasePrice);
        Task<IReadOnlyCollection<PurchasePrice>> GetFor(int supplierId, int productId);
    }
}
