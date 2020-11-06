using System;
using System.Threading.Tasks;

using FluentAssertions;

using Moq;

using PurchasePrices.Domain.Entities;
using PurchasePrices.Domain.Ports;
using PurchasePrices.Domain.Usecases;
using PurchasePrices.Tests;

using Xunit;

namespace WhenSavingPurchasePrice
{
    public class GivenAValidTemporaryPrice
    {
        public class AndNoExistingTemporaryPrice
        {
            [Fact]
            public async Task ShouldSavePurchasePrice()
            {
                var purchasePrice = new PurchasePriceBuilder()
                    .ATemporaryPrice()
                    .Build();

                var purchasePriceRepositoryMock = new Mock<IPurchasePriceRepository>();
                //purchasePriceRepositoryMock
                //    .Setup(a => a.GetFor(It.IsAny<int>(), It.IsAny<int>()))
                //    .ReturnsAsync(Array.Empty<PurchasePrice>());

                var sut = new SavePurchasePrice(purchasePriceRepositoryMock.Object);

                await sut.Invoke(purchasePrice);

                purchasePriceRepositoryMock.Verify(a => a.Save(purchasePrice), Times.Once);
            }
        }

        public class AndThereIsUpcomingTemporaryPrice
        {
            [Fact]
            public async Task ShouldDeleteExistingUpcomingTemporaryPriceAndSavePurchasePrice()
            {
                var purchasePrice = new PurchasePriceBuilder()
                    .ATemporaryPrice()
                    .Build();

                var existingUpcomingPurchasePrice = new PurchasePriceBuilder().ATemporaryPrice()
                    .WithStartDate(DateTime.Now.AddDays(10))
                    .WithEndDate(DateTime.Now.AddDays(20))
                    .Build();

                var purchasePriceRepositoryMock = new Mock<IPurchasePriceRepository>();
                purchasePriceRepositoryMock
                    .Setup(a => a.GetFor(It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(new[] { existingUpcomingPurchasePrice });

                var sut = new SavePurchasePrice(purchasePriceRepositoryMock.Object);

                await sut.Invoke(purchasePrice);

                purchasePriceRepositoryMock
                    .Verify(a => a.GetFor(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
                
                purchasePriceRepositoryMock
                    .Verify(a => a.Delete(existingUpcomingPurchasePrice), Times.Once);

                purchasePriceRepositoryMock
                    .Verify(a => a.Save(purchasePrice), Times.Once);
            }
        }
    }

    public class GivenThereAreNoPricesWhenUploadingTemporaryPriceThatStartsToday
    {
        [Fact]
        public void ShouldBecomeActivePrice()
        {
            var purchasePrice = new PurchasePriceBuilder()
                .ATemporaryPrice()
                .Build();

            purchasePrice.IsActive.Should().BeTrue();
            purchasePrice.IsUpcoming.Should().BeFalse();
        }
    }
    
    public class GivenThereAreNoPricesWhenUploadingPermanentPriceThatStartsToday
    {
        [Fact]
        public void ShouldBecomeActivePrice()
        {
            var purchasePrice = new PurchasePriceBuilder()
                .WithStartDate(DateTime.Today)
                .APermanentPrice()
                .Build();

            purchasePrice.IsActive.Should().BeTrue();
            purchasePrice.IsUpcoming.Should().BeFalse();
        }
    }
    
    public class GivenThereAreNoPricesWhenUploadingTemporaryPriceThatStartsTomorrow
    {
        [Fact]
        public void ShouldBecomeUpcomingPrice()
        {
            var purchasePrice = new PurchasePriceBuilder()
                .WithStartDate(DateTime.Today.AddDays(1))
                .ATemporaryPrice()
                .Build();

            purchasePrice.IsActive.Should().BeFalse();
            purchasePrice.IsUpcoming.Should().BeTrue();
        }
    }
    
    public class GivenThereAreNoPricesWhenUploadingPermanentPriceThatStartsTomorrow
    {
        [Fact]
        public void ShouldBecomeUpcomingPrice()
        {
            var purchasePrice = new PurchasePriceBuilder()
                .WithStartDate(DateTime.Today.AddDays(1))
                .APermanentPrice()
                .Build();

            purchasePrice.IsActive.Should().BeFalse();
            purchasePrice.IsUpcoming.Should().BeTrue();
        }
    }
}
