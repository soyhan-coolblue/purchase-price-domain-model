using System;

namespace PurchasePrices.Domain.Entities
{
    public class PurchasePrice
    {
        public int SupplierId { get; }
        public PriceType PriceType { get; }
        public int ProductId { get; }
        public decimal Price { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }

        public bool IsActive => StartDate <= DateTime.Today && (EndDate == null || DateTime.Today <= EndDate);
        public bool IsUpcoming => StartDate > DateTime.Today;

        public PurchasePrice(int supplierId, PriceType priceType, int productId, decimal price, DateTime startDate, DateTime? endDate)
        {
            if(supplierId<=0)
                throw new ArgumentOutOfRangeException($"{nameof(supplierId)} should be greater than 0.");

            SupplierId = supplierId;
            PriceType = priceType;

            if (productId <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(ProductId)} should be greater than 0.");

            ProductId = productId;

            if (price <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(Price)} should be greater than 0.");
            
            Price = price;

            StartDate = startDate;

            if (PriceType == PriceType.Permanent && endDate.HasValue)
                throw new ArgumentOutOfRangeException($"{nameof(EndDate)} is not allowed for Permanent prices.");

            if(PriceType == PriceType.Temporary && endDate.HasValue == false)
                throw new ArgumentOutOfRangeException($"For temporary prices, {nameof(EndDate)} is mandatory.");

            if (startDate > endDate)
                throw new ArgumentOutOfRangeException($"{nameof(StartDate)} cannot be after {nameof(EndDate)}.");

            EndDate = endDate;
        }
    }
}