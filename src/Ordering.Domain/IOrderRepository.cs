using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        void Update(Order order);
        Task<Order> GetAsync(Guid orderGuid);
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken);
    }
}
