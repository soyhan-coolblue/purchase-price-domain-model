using System;

using FluentAssertions;

using PurchasePrices.Domain.Entities;

using Xunit;

namespace WhenConstructingPurchasePriceEntity
{
    public class ShouldThrowException
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivenSupplierIdIsInvalid(int supplierId)
        {
            Action action = () =>
            {
                var purchasePrice = new PurchasePrice(supplierId, PriceType.Permanent, 1, 1, DateTime.Now, null);
            };

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivenProductIdIsInvalid(int productId)
        {
            Action action = () =>
            {
                var purchasePrice = new PurchasePrice(1, PriceType.Permanent, productId, 1, DateTime.Now, null);
            };

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivePriceIsInvalid(decimal price)
        {
            Action action = () =>
            {
                var purchasePrice = new PurchasePrice(1, PriceType.Permanent, 1, price, DateTime.Now, null);
            };

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        
        public class GivenPriceTypeIsPermanent
        {
            [Fact]
            public void AndEndDateIsSet()
            {
                DateTime startDate = DateTime.Today.AddDays(-1);
                DateTime endDate = DateTime.Today;
                Action action = () =>
                {
                    var purchasePrice = new PurchasePrice(1, PriceType.Permanent, 1, 1, startDate, endDate);
                };

                action.Should().ThrowExactly<ArgumentOutOfRangeException>();
            }
        }
        
        public class GivenPriceTypeIsTemporary
        {
            [Fact]
            public void AndEndDateIsNotSet()
            {
                DateTime startDate = DateTime.Today.AddDays(-1);
                
                Action action = () =>
                {
                    var purchasePrice = new PurchasePrice(1, PriceType.Temporary, 1, 1, startDate, DateTime.Today.AddDays(20));
                };

                action.Should().ThrowExactly<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void AndEndDateIsBeforeStartDate()
            {
                DateTime startDate = DateTime.Today.AddDays(-1);
                DateTime endDate = startDate.AddDays(-1);

                Action action = () =>
                {
                    var purchasePrice = new PurchasePrice(1, PriceType.Temporary, 1, 1, startDate, endDate);
                };

                action.Should().ThrowExactly<ArgumentOutOfRangeException>();
            }
        }
    }

    public class ShouldNotThrowException
    {
        [Fact]
        public void GivenSupplierIdIsValid()
        {
            var supplierId = 1;
            Action action = () =>
            {
                var purchasePrice = new PurchasePrice(supplierId, PriceType.Permanent, 1, 1, DateTime.Now, null);
            };

            action.Should().NotThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GivenProductIdIsValid()
        {
            var productId = 1;
            Action action = () =>
            {
                var purchasePrice = new PurchasePrice(1, PriceType.Permanent, productId, 1, DateTime.Now, null);
            };

            action.Should().NotThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GivenPriceIsValid()
        {
            decimal price = 1.2M;
            Action action = () =>
            {
                var purchasePrice = new PurchasePrice(1, PriceType.Permanent, 1, price, DateTime.Now, null);
            };

            action.Should().NotThrow<ArgumentOutOfRangeException>();
        }
    }

    public class ShouldBeActive
    {
        [Fact]
        public void GivenStartDateIsToday()
        {
            var purchasePrice = new PurchasePrice(1, PriceType.Permanent, 1, 1, DateTime.Today.AddDays(-1), null);

            purchasePrice.IsActive.Should().BeTrue();
            purchasePrice.IsUpcoming.Should().BeFalse();
        }

        [Fact]
        public void GivenStartDateIsBeforeToday()
        {
            var purchasePrice = new PurchasePrice(1, PriceType.Permanent, 1, 1, DateTime.Today.AddDays(-1), null);

            purchasePrice.IsActive.Should().BeTrue();
            purchasePrice.IsUpcoming.Should().BeFalse();
        }
    }

    public class ShouldBeUpcoming
    {
        [Fact]
        public void GivenStartDateIsInFuture()
        {
            var purchasePrice = new PurchasePrice(1, PriceType.Permanent, 1, 1, DateTime.Today.AddDays(1), null);

            purchasePrice.IsActive.Should().BeFalse();
            purchasePrice.IsUpcoming.Should().BeTrue();
        }
    }
}
