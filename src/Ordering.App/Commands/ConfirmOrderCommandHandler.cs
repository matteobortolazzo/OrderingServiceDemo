using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.App.Commands
{
    public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, bool>
    {
        private readonly IOrderRepository _repository;

        public ConfirmOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetAsync(request.Identifier);

            if (order == null)
            {
                return false;
            }

            order.Confirm();
            _repository.Update(order);

            return await _repository.SaveEntitiesAsync(cancellationToken);
        }
    }
}
