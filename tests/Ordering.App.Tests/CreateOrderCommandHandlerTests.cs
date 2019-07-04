using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Ordering.App.Commands;
using Ordering.App.Queries;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Xunit;

namespace Ordering.App.Tests
{
    public class CreateOrderCommandHandlerTests
    {
        private const int _maxPendingAmount = 100;
        private const int _maxPendingProducts = 10;
        private readonly Mock<IOrderRepository> _repository;
        private readonly Mock<IOrderQueries> _queries;
        private readonly Mock<IOptions<OrderOptions>> _orderOptions;

        public CreateOrderCommandHandlerTests()
        {
            _repository = new Mock<IOrderRepository>();
            _queries = new Mock<IOrderQueries>();
            _orderOptions = new Mock<IOptions<OrderOptions>>();

            _orderOptions
                .Setup(o => o.Value)
                .Returns(new OrderOptions
                {
                    MaxPendingAmount = _maxPendingAmount,
                    MaxPendingProducts = _maxPendingProducts
                });            
        }

        private CreateOrderCommand NewCommand(int quantity)
        {
            return new CreateOrderCommand
            {
                UserId = 0,
                ProductId = 0,
                Quantity = quantity,
                Street = "A",
                City = "A",
                State = "A",
                ZipCode = "A"
            };
        }

        private CreateOrderCommandHandler NewCommandHandler()
        {
            return new CreateOrderCommandHandler(_repository.Object, _queries.Object, _orderOptions.Object);
        }

        [Fact]
        public async Task ValidOrder_Returns_True()
        {
            // Arrange
            var command = NewCommand(quantity: 1);
            var commandHandler = NewCommandHandler();
            _queries
                .Setup(q => q.HasExceedMaxPendingAmount(command.UserId, _maxPendingProducts))
                .ReturnsAsync(false);
            _queries
                .Setup(q => q.HasExeedMaxPendingProducts(command.UserId, command.ProductId, _maxPendingProducts))
                .ReturnsAsync(false);
            _repository
                .Setup(r => r.SaveEntitiesAsync(CancellationToken.None))
                .ReturnsAsync(true);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task OrderMoreThanMaxProducts_Returns_False()
        {
            // Arrange
            var command = NewCommand(quantity: _maxPendingProducts + 1);
            var commandHandler = NewCommandHandler();

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task TooManyPendingProductOrders_Returns_False()
        {
            // Arrange
            var command = NewCommand(quantity: 1);
            var commandHandler = NewCommandHandler();
            _queries
                .Setup(q => q.HasExeedMaxPendingProducts(command.UserId, command.ProductId, _maxPendingProducts))
                .ReturnsAsync(true);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task TooManyPendingOrders_Returns_False()
        {
            // Arrange
            var command = NewCommand(quantity: 1);
            var commandHandler = NewCommandHandler();
            _queries
                .Setup(q => q.HasExceedMaxPendingAmount(command.UserId, _maxPendingProducts))
                .ReturnsAsync(true);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }
}
