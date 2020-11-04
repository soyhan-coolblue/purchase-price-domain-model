using System;

using PurchasePrices.Domain.Entities;

namespace PurchasePrices.Tests
{
    public class PurchasePriceBuilder
    {
        private int _supplierId;
        private int _productId;
        private decimal _price;
        private DateTime _startDate;
        private DateTime _endDate;
        private PriceType _priceType;

        public PurchasePriceBuilder()
        {
            _supplierId = 1;
            _productId = 1;
            _price = 1;
            _startDate = DateTime.Today;
            _endDate = DateTime.Today.AddDays(10);
        }

        public PurchasePriceBuilder ATemporaryPrice()
        {
            _priceType = PriceType.Temporary;
            return this;
        }

        public PurchasePrice Build() =>
            new PurchasePrice(_supplierId, _priceType, _productId, _price, _startDate, _endDate);

        public PurchasePriceBuilder WithStartDate(DateTime startDate)
        {
            _startDate = startDate;

            return this;
        }
        public PurchasePriceBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;

            return this;
        }
    }
}