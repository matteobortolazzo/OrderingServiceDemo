using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Ordering.App.Queries;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.App.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderQueries _queries;
        private readonly OrderOptions _orderOptions;

        public CreateOrderCommandHandler(IOrderRepository repository, IOrderQueries queries, IOptions<OrderOptions> options)
        {
            _repository = repository;
            _queries = queries;
            _orderOptions = options.Value;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Random price
            var price = new Random().Next(10, 50);

            var identifier = Guid.NewGuid();
            var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            var order = new Order(identifier, request.Quantity, price, address, request.ProductId, request.UserId);

            var maxPendingProducts = _orderOptions.MaxPendingProducts - request.Quantity;
            if (maxPendingProducts <= 0)
            {
                return false;
            }

            bool isValid = true;
            if (await _queries.HasExceedMaxPendingAmount(request.UserId, _orderOptions.MaxPendingAmount))
            {
                order.Reject(OrderRejectionReason.TooManyPendingOrders);
                isValid = false;
            }
            else if (await _queries.HasExeedMaxPendingProducts(request.UserId, request.ProductId, maxPendingProducts))
            {
                order.Reject(OrderRejectionReason.TooManyProductsOrdered);
                isValid = false;
            }

            await _repository.AddAsync(order);

            var isSaved = await _repository.SaveEntitiesAsync(cancellationToken);
            return isValid && isSaved;
        }
    }
}
